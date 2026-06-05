using LoanApplicationAPI.Models;
using LoanApplicationAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// SERVICES
// ============================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, UserRoles>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// ============================================
// CORS
// ============================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://loanshark-jd1g.vercel.app",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// ============================================
// MIDDLEWARE
// ============================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable Swagger on Render too
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ============================================
// DATABASE MIGRATION + ROLE SEEDING
// ============================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext =
            services.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();

        var roleManager =
            services.GetRequiredService<RoleManager<UserRoles>>();

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

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

        var adminEmail = "admin@loan.co.za";

        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                adminUser,
                "Admin@123456"
            );

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin"
                );
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

// ============================================
// START APPLICATION
// ============================================

app.Run();