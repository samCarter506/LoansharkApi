using LoanApplicationAPI.Models;
using LoanApplicationAPI.Services;
using LoanApplicationLibrary.Data;
using LoanApplicationLibrary.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// DATABASE
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ==========================================
// IDENTITY
// ==========================================
builder.Services
    .AddIdentity<ApplicationUser, UserRoles>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ==========================================
// JWT AUTHENTICATION
// ==========================================
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["JwtSettings:Issuer"],

                ValidAudience =
                    builder.Configuration["JwtSettings:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["JwtSettings:SecretKey"]
                        )
                    )
            };

        // READ TOKEN FROM COOKIE
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token =
                    context.Request.Cookies["access_token"];

                return Task.CompletedTask;
            }
        };
    });

// ==========================================
// AUTHORIZATION
// ==========================================
builder.Services.AddAuthorization();

// ==========================================
// CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000", "https://loanshark-phi.vercel.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// ==========================================
// DEPENDENCY INJECTION
// ==========================================
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IDataAccess, DataAccess>();

builder.Services.AddScoped<IApplicationData, ApplicationData>();

builder.Services.AddScoped<IBankingData, BankingData>();

builder.Services.AddScoped<IEmployerData, EmployerData>();
builder.Services.AddScoped<ILoanCalculationData, LoanCalculationData>();

// ==========================================
// CONTROLLERS
// ==========================================
builder.Services.AddControllers();

// ==========================================
// SWAGGER
// ==========================================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================
// CREATE DEFAULT ROLES
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<UserRoles>>();

    string[] roles =
    {
        "Admin",
        "User"
    };

    foreach (var role in roles)
    {
        var roleExists =
            await roleManager.RoleExistsAsync(role);

        if (!roleExists)
        {
            await roleManager.CreateAsync(
                new UserRoles
                {
                    Name = role
                });
        }
    }
}

// ==========================================
// MIDDLEWARE
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();