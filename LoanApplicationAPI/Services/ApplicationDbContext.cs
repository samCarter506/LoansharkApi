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


        // =========================
        // ROLES
        // =========================
        public new DbSet<UserRoles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            //==========================
            // EXPENSES
            //==========================
            modelBuilder.Entity<ExpensesModel>()
                .HasOne(f => f.application).WithMany()
                .HasForeignKey(f => f.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // SYSTEM CODE DETAILS
            // =========================

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
            // APPLICATION -> LOAN

            modelBuilder.Entity<ApplicationModel>()
                .HasOne(a => a.Loan)
                .WithOne(l => l.application)
                .HasForeignKey<LoanModel>(l => l.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // APPLICATION -> EXPENSES

            modelBuilder.Entity<ApplicationModel>()
                .HasOne(a => a.Expenses)
                .WithOne(e => e.application)
                .HasForeignKey<ExpensesModel>(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

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
            modelBuilder.Entity<ApplicationDocument>()
    .HasOne(d => d.Application)
    .WithMany(a => a.Documents)
    .HasForeignKey(d => d.ApplicationId)
    .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // APPLICATION -> BANKING
            // =========================

            modelBuilder.Entity<ApplicationModel>()
                .HasOne(a => a.Banking)
                .WithOne(b => b.application)
                .HasForeignKey<BankingModel>(b => b.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // APPLICATION -> EMPLOYER
            // =========================

            modelBuilder.Entity<ApplicationModel>()
                .HasOne(a => a.Employer)
                .WithOne(e => e.application)
                .HasForeignKey<EmployerModel>(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);



            // =========================
            // DECIMAL PRECISION
            // =========================

            modelBuilder.Entity<LoanModel>()
       .Property(l => l.monthlyPayment)
       .HasPrecision(18, 2);

            modelBuilder.Entity<StatusDto>()
                .HasNoKey();
        }
    }
}