using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;
using WebApplicationWithIdentity;
using WebApplicationWithIdentity.Models;

namespace WebApiWithIdentity
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ============================================
            // Add Services
            // ============================================

            // Add Controllers
            builder.Services.AddControllers();

            // Add Memory Cache (for temporary 2FA tokens & verification)
            builder.Services.AddMemoryCache();

            // Add HttpClient for calling external provider APIs (Google, Microsoft, GitHub, Facebook)
            builder.Services.AddHttpClient();

            // Register Custom Authorization Handlers


            // Configure Email Settings & Services


            // ============================================
            // Add Swagger / OpenAPI
            // ============================================

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Identity Demo API",
                    Version = "v1",
                    Description = "Authentication and Authorization built on ASP.NET Core Identity"
                });
            });

            // ============================================
            // Configure Entity Framework Core
            // ============================================

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // ============================================
            // Configure Identity
            // ============================================

            builder.Services.AddIdentity<AppUser, AppRole>(
                options =>
                {
                    // ----------------------------------------
                    // Password Settings
                    // ----------------------------------------

                    options.Password.RequireDigit = true;

                    options.Password.RequireLowercase = true;

                    options.Password.RequireUppercase = true;

                    options.Password.RequireNonAlphanumeric = true;

                    options.Password.RequiredLength = 10;

                    options.Password.RequiredUniqueChars = 3;

                    // ----------------------------------------
                    // Lockout Settings
                    // ----------------------------------------

                    options.Lockout.DefaultLockoutTimeSpan =
                        TimeSpan.FromMinutes(30);

                    options.Lockout.MaxFailedAccessAttempts = 2;

                    options.Lockout.AllowedForNewUsers = true;

                    // ----------------------------------------
                    // User Settings
                    // ----------------------------------------

                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyz" +
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                        "0123456789-._@+";

                    options.User.RequireUniqueEmail = true;

                    // ----------------------------------------
                    // Sign-in Settings
                    // ----------------------------------------

                    options.SignIn.RequireConfirmedEmail = false;

                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            // ============================================
            // Configure Application Cookie (Web API friendly)
            // ============================================

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            });

            // ============================================
            // Configure Authorization
            // ============================================

            builder.Services.AddAuthorization(options =>
            {
                // ----------------------------------------
                // Adult Content Policy
                // ----------------------------------------



                // ----------------------------------------
                // Department Finance Policy
                // ----------------------------------------

                options.AddPolicy("DepartmentFinance",
                    policy =>
                        policy.RequireClaim(
                            "Department",
                            "Finance"));

                // ----------------------------------------
                // Has Employee ID Policy
                // ----------------------------------------

                options.AddPolicy("HasEmployeeId",
                    policy =>
                        policy.RequireClaim("EmployeeId"));

                // ----------------------------------------
                // Senior Staff Policy
                // ----------------------------------------

                options.AddPolicy("SeniorStaff",
                    policy =>
                        policy.RequireClaim(
                            "Level",
                            "Senior",
                            "Lead",
                            "Manager",
                            "Director"));

                // ----------------------------------------
                // Premium Access Policy
                // ----------------------------------------



                // ============================================
                // Build Application
                // ============================================
            });



            var app = builder.Build();



                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();

                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                // Authentication

                app.UseAuthentication();

                // Authorization

                app.UseAuthorization();

                // Map Controllers

                app.MapControllers();

                // Run Application

                app.Run();
           


            
        }

   
    }
}