using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class AppBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private DateTime _lastNoonReminder = DateTime.MinValue;
        private DateTime _lastEveningReminder = DateTime.MinValue;

       

        public AppBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {

                DateTime now = DateTime.UtcNow;
                DateTime today = now.Date;

                DateTime noon = today.AddHours(12);
                DateTime evening = today.AddHours(22);

                using var scope = _scopeFactory.CreateScope();

                var taskService = scope.ServiceProvider.GetRequiredService<ITaskItemService>();
                var emailCodeService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var challengeService = scope.ServiceProvider.GetRequiredService<IChallengeService>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                await emailCodeService.DeleteCodes();

                await taskService.MarkOverdueTasksAsync();
                await taskService.DeleteOverdueTasksMoreThanDay();
                await userService.SendReminderForTasksEmail();
                await userService.DeleteUserRelationByMoreThanAMonth();
                await challengeService.ChooseRandomChallenges();
                await challengeService.RemoveChallengesActivity();

                if (now >= noon && _lastNoonReminder != today)
                {
                    await userService.SendReminderForStreakEmail();
                    _lastNoonReminder = today;
                }
                if (now >= evening && _lastEveningReminder != today)
                {
                    await userService.SendReminderForStreakEmail();
                    _lastEveningReminder = today;
                }
                // Wait 1 minute before checking again
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
