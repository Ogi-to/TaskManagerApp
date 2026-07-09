using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class OverdueTaskBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OverdueTaskBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var taskService = scope.ServiceProvider.GetRequiredService<ITaskItemService>();

                await taskService.MarkOverdueTasksAsync();

                // Wait 5 minutes before checking again
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
