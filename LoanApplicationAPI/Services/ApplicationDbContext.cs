using LoanApplicationAPI.Models;
using LoanApplicationAPI.Models.DTO;
using LoanApplicationLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationAPI.Services
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, UserRoles, string>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =========================
        // SYSTEM TABLES
        // =========================

        public DbSet<SystemCode> SystemCode { get; set; }
        public DbSet<SystemCodeDetails> SystemCodeDetails { get; set; }
        public DbSet<UserAudit> Audit { get; set; }
        public DbSet<ApplicationDocument> Documents { get; set; }
        public DbSet<StatusDto> StatusDtos { get; set; }

        // =========================
        // APPLICATION TABLES
        // =========================

        public DbSet<ApplicationModel> Applications { get; set; }
        public DbSet<EmployerModel> Employers { get; set; }
        public DbSet<BankingModel> Bankings { get; set; }
        public DbSet<ExpensesModel> MonthlyExpense { get; set; }
        public DbSet<LoanModel> Loan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // TABLE NAMES
            // =========================

            modelBuilder.Entity<ApplicationModel>()
                .ToTable("Applications");

            modelBuilder.Entity<EmployerModel>()
                .ToTable("Employers");

            modelBuilder.Entity<BankingModel>()
                .ToTable("Bankings");

            modelBuilder.Entity<ExpensesModel>()
                .ToTable("MonthlyExpense");

            modelBuilder.Entity<LoanModel>()
                .ToTable("Loan");

            modelBuilder.Entity<ApplicationDocument>()
                .ToTable("Documents");

            modelBuilder.Entity<UserAudit>()
                .ToTable("Audit");

            // =========================
            // SYSTEM CODE RELATIONSHIPS
            // =========================

            modelBuilder.Entity<SystemCode>()
                .HasOne(f => f.CreatedUser)
                .WithMany()
                .HasForeignKey(f => f.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemCode>()
                .HasOne(f => f.ModifiedUser)
                .WithMany()
                .HasForeignKey(f => f.ModifiedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemCodeDetails>()
                .HasOne(f => f.CreatedUser)
                .WithMany()
                .HasForeignKey(f => f.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemCodeDetails>()
                .HasOne(f => f.ModifiedUser)
                .WithMany()
                .HasForeignKey(f => f.ModifiedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemCodeDetails>()
                .HasOne(f => f.SystemCode)
                .WithMany()
                .HasForeignKey(f => f.SystemCodeId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // AUDIT
            // =========================

            modelBuilder.Entity<UserAudit>()
                .HasOne(f => f.CreatedUser)
                .WithMany()
                .HasForeignKey(f => f.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserAudit>()
                .HasOne(f => f.ModifiedUser)
                .WithMany()
                .HasForeignKey(f => f.ModifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // APPLICATION USER LOOKUPS
            // =========================

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(f => f.Gender)
                .WithMany()
                .HasForeignKey(f => f.GenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(f => f.Race)
                .WithMany()
                .HasForeignKey(f => f.RaceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(f => f.Province)
                .WithMany()
                .HasForeignKey(f => f.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(f => f.Language)
                .WithMany()
                .HasForeignKey(f => f.HomeLanguage)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // APPLICATION -> LOAN
            // =========================

            modelBuilder.Entity<LoanModel>()
                .HasIndex(l => l.ApplicationId)
                .IsUnique();

            modelBuilder.Entity<ApplicationModel>()
                .HasOne(a => a.Loan)
                .WithOne(l => l.application)
                .HasForeignKey<LoanModel>(l => l.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // APPLICATION -> EXPENSES
            // =========================

            modelBuilder.Entity<ExpensesModel>()
                .HasIndex(e => e.ApplicationId)
                .IsUnique();

            modelBuilder.Entity<ApplicationModel>()
                .HasOne(a => a.Expenses)
                .WithOne(e => e.application)
                .HasForeignKey<ExpensesModel>(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // APPLICATION -> BANKING
            // =========================

            modelBuilder.Entity<BankingModel>()
                .HasIndex(b => b.ApplicationId)
                .IsUnique();

            modelBuilder.Entity<ApplicationModel>()
                .HasOne(a => a.Banking)
                .WithOne(b => b.application)
                .HasForeignKey<BankingModel>(b => b.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // APPLICATION -> EMPLOYER
            // =========================

            modelBuilder.Entity<EmployerModel>()
                .HasIndex(e => e.ApplicationId)
                .IsUnique();

            modelBuilder.Entity<ApplicationModel>()
                .HasOne(a => a.Employer)
                .WithOne(e => e.application)
                .HasForeignKey<EmployerModel>(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // APPLICATION -> DOCUMENTS
            // =========================

            modelBuilder.Entity<ApplicationDocument>()
                .HasOne(d => d.Application)
                .WithMany(a => a.Documents)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // DECIMAL PRECISION
            // =========================

            modelBuilder.Entity<LoanModel>()
                .Property(l => l.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanModel>()
                .Property(l => l.monthlyPayment)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanModel>()
                .Property(l => l.RemainingBalance)
                .HasPrecision(18, 2);

            // =========================
            // STORED PROCEDURE DTO
            // =========================

            modelBuilder.Entity<StatusDto>()
                .HasNoKey();
        }
    }
}