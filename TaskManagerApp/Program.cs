using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Interfaces;
using TaskManagerApp.Repositories;

namespace TaskManagerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

<<<<<<< Updated upstream
            // DbContext
            builder.Services.AddDbContext<TaskManagerDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

=======
>>>>>>> Stashed changes
            // Controllers
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext
            builder.Services.AddDbContext<TaskManagerDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

<<<<<<< Updated upstream
            // Swagger UI
=======
            // Swagger
>>>>>>> Stashed changes
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
<<<<<<< Updated upstream

            app.UseAuthorization();
=======

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapGet("/", () => "Task Manager API Running");
>>>>>>> Stashed changes

            app.MapControllers();

            app.Run();
        }
    }
}