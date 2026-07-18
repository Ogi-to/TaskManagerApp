using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IUserService
    {
        public Task RegisterUser(RegisterUserDto registerUserDto);
        public Task<UserDto> LogInUser(LoginUserDto loginUserDto);
        public Task<UserDto> GetUserById(int id);
        public Task DeleteAccount(int userId);
        public Task VerifyEmail(VerifyEmailDto verifyEmailDto);
        public Task SendReminderForTasksEmail();
        public Task UpdateStreak(User user);
        public Task UpdateRank(User user);
        public Task UpdatePoints(int userId, int points);
        public Task UpdateReminderSettings(int userId, ReminderSettingsDto reminderSettingsDto);
    }
}
