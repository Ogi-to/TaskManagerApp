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
                var emailCodeService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var challengeService = scope.ServiceProvider.GetRequiredService<IChallengeService>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                await emailCodeService.DeleteCodes();

                await taskService.MarkOverdueTasksAsync();
                await taskService.DeleteOverdueTasksMoreThanDay();
                await userService.SendReminderEmail();

                await challengeService.ChooseRandomChallenges();
                await challengeService.RemoveChallengesActivity();
                // Wait 1 minute before checking again
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
