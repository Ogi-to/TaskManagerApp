using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

using TaskManagerApp.Data;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesRepositories;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;
using TaskManagerApp.Services;

namespace TaskManagerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==========================================
            // SHARED COOKIE ENCRYPTION KEYS
            // ==========================================

            builder.Services
                .AddDataProtection()
                .PersistKeysToFileSystem(
                    new DirectoryInfo(
                        @"C:\TaskRush\AuthKeys"))
                .SetApplicationName("TaskRush");


            // ==========================================
            // DATABASE
            // ==========================================

            builder.Services.AddDbContext<TaskManagerDbContext>(
                options =>
                    options.UseNpgsql(
                        builder.Configuration
                            .GetConnectionString("DefaultConnection")));


            // ==========================================
            // AUTHENTICATION - COOKIE
            // ==========================================

            builder.Services
                .AddAuthentication(
                    CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // ----------------------------------
                    // COOKIE
                    // ----------------------------------

                    options.Cookie.Name = "TaskRush.Auth";

                    options.Cookie.Path = "/";

                    options.Cookie.HttpOnly = true;

                    options.Cookie.SecurePolicy =
                        CookieSecurePolicy.Always;

                    options.Cookie.SameSite =
                        SameSiteMode.Lax;


                    // ----------------------------------
                    // EXPIRATION
                    // ----------------------------------

                    options.ExpireTimeSpan =
                        TimeSpan.FromDays(30);

                    options.SlidingExpiration = true;


                    // ----------------------------------
                    // API 401 вместо redirect към /login
                    // ----------------------------------

                    options.Events.OnRedirectToLogin =
                        context =>
                        {
                            context.Response.StatusCode = 401;

                            return Task.CompletedTask;
                        };


                    // ----------------------------------
                    // API 403 вместо redirect
                    // ----------------------------------

                    options.Events.OnRedirectToAccessDenied =
                        context =>
                        {
                            context.Response.StatusCode = 403;

                            return Task.CompletedTask;
                        };
                });


            // ==========================================
            // AUTHORIZATION
            // ==========================================

            builder.Services.AddAuthorization();


            // ==========================================
            // CONTROLLERS
            // ==========================================

            builder.Services.AddControllers();


            // ==========================================
            // REPOSITORIES
            // ==========================================

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<
                IUserStatsRepository,
                UserStatsRepository>();

            builder.Services.AddScoped<
                ITaskItemRepository,
                TaskItemRepository>();

            builder.Services.AddScoped<
                IStateRepository,
                StateRepository>();

            builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();

            builder.Services.AddScoped<
            IFriendshipRepository,
            FriendshipRepository>();

            builder.Services.AddScoped<
                IRankRepository,
                RankRepository>();

            builder.Services.AddScoped<
                IChallengeRepository,
                ChallengeRepository>();

            builder.Services.AddScoped<
                ICategoryRepository,
                CategoryRepository>();

            builder.Services.AddScoped<
                IEmailCodeRepository,
                EmailCodeRepository>();


            // ==========================================
            // SERVICES
            // ==========================================

            builder.Services.AddScoped<
                IUserService,
                UserService>();

            builder.Services.AddScoped<
                ITaskItemService,
                TaskItemService>();

            builder.Services.AddScoped<
            ICalendarService,
            CalendarService>();

            builder.Services.AddScoped<
            IFriendshipService,
            FriendshipService>();

            builder.Services.AddScoped<
                IEmailCodeService,
                EmailCodeService>();

            builder.Services.AddScoped<HashPasswordService>();

            builder.Services.AddScoped<
                ICategoryService,
                CategoryService>();

            builder.Services.AddScoped<
                IChallengeService,
                ChallengeService>();

            builder.Services.AddScoped<
                IStateService,
                StateService>();

            builder.Services.AddScoped<
                IRankService,
                RankService>();

            builder.Services.AddScoped<
                IUserStatsService,
                UserStatsService>();


            // ==========================================
            // BACKGROUND SERVICE
            // ==========================================

            builder.Services.AddHostedService<
                AppBackgroundService>();


            // ==========================================
            // SWAGGER
            // ==========================================

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();


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
                            .WithOrigins(
                                "https://localhost:7254")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });


            // ==========================================
            // BUILD APP
            // ==========================================

            var app = builder.Build();


            // ==========================================
            // SWAGGER
            // ==========================================

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }


            // ==========================================
            // HTTPS
            // ==========================================

            app.UseHttpsRedirection();


            // ==========================================
            // CORS
            // ==========================================

            app.UseCors("AllowFrontend");


            // ==========================================
            // EXCEPTION MIDDLEWARE
            // ==========================================

            app.UseExceptionMiddleware();


            // ==========================================
            // AUTHENTICATION
            // ==========================================

            app.UseAuthentication();


            // ==========================================
            // AUTHORIZATION
            // ==========================================

            app.UseAuthorization();


            // ==========================================
            // ROOT
            // ==========================================

            app.MapGet(
                "/",
                () => "TaskRush API Running");


            // ==========================================
            // CONTROLLERS
            // ==========================================

            app.MapControllers();


            // ==========================================
            // RUN
            // ==========================================

            app.Run();
        }
    }
}