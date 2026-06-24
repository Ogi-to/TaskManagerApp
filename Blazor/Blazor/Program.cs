using Blazor.Client.Pages;
using Blazor.Components;
using Microsoft.AspNetCore.Components;
using TaskManagerApp.Interfaces;
using TaskManagerApp.Repositories;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;

namespace Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();



            builder.Services.AddControllers();
            builder.Services.AddDbContext<TaskManagerDbContext>(options =>
              options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add HttpClient correctly
            builder.Services.AddScoped<HttpClient>(sp =>
            {
                var navigationManager = sp.GetRequiredService<NavigationManager>();

                return new HttpClient
                {
                    BaseAddress = new Uri(navigationManager.BaseUri)
                };
            });
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();

            app.MapControllers();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
