using LoanApplicationAPI.Models;
using LoanApplicationAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, UserRoles>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// ============================================
// DATABASE + ROLE + ADMIN SEEDING
// ============================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var db =
            services.GetRequiredService<ApplicationDbContext>();

        // FIRST
        await db.Database.MigrateAsync();

        var roleManager =
            services.GetRequiredService<RoleManager<UserRoles>>();

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        // ==========================================
        // CREATE ROLES
        // ==========================================

        string[] roles =
        {
            "Admin",
            "User"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new UserRoles
                    {
                        Name = role
                    });
            }
        }

        // ==========================================
        // CREATE ADMIN
        // ==========================================

        var adminEmail = "admin@loan.co.za";

        var admin =
            await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System Administrator",
                LastName= "Administrator",
                EmailConfirmed = true
            };

            var result =
                await userManager.CreateAsync(
                    admin,
                    "Admin@123456"
                );

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    admin,
                    "Admin"
                );
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}

app.Run();