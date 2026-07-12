using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class AppBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AppBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var taskService = scope.ServiceProvider.GetRequiredService<ITaskItemService>();
                var emailCodeService = scope.ServiceProvider.GetRequiredService<IEmailCodeService>();
                var challengeService = scope.ServiceProvider.GetRequiredService<IChallengeService>();

                await emailCodeService.DeleteCodes();

                await taskService.MarkOverdueTasksAsync();
                await taskService.DeleteOverdueTasksMoreThanDay();

                await challengeService.ChooseRandomChallenges();
                await challengeService.RemoveChallengesActivity();
                // Wait 5 minutes before checking again
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
