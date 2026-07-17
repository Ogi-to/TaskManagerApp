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

            // DbContext
            builder.Services.AddDbContext<TaskManagerDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Controllers
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserStatsRepository, UserStatsRepository>();
            builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            builder.Services.AddScoped<IStateRepository, StateRepository>();
            builder.Services.AddScoped<IRankRepository, RankRepository>();
            builder.Services.AddScoped<IChallengeRepository, ChallengeRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IEmailCodeRepository, EmailCodeRepository>();


            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskItemService, TaskItemService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<HashPasswordService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IChallengeService, ChallengeService>();
            builder.Services.AddScoped<IStateService, StateService>();
            builder.Services.AddScoped<IRankService, RankService>();
            builder.Services.AddScoped<IUserStatsService, UserStatsService>();

            builder.Services.AddHostedService<AppBackgroundService>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseExceptionMiddleware();

            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapGet("/", () => "Task Manager API Running");


            app.MapControllers();

            app.Run();
        }
    }
}