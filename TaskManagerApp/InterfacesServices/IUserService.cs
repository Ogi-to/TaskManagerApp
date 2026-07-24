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
        public Task SendReminderForStreakEmail();
        public Task UpdateStreak(UpdateUserDto user);
        public Task UpdateRank(UpdateUserDto user);
        public Task UpdatePoints(int userId, int points);
        public Task UpdateReminderSettings(ReminderSettingsDto reminderSettingsDto);
        public Task ReSendVerificationCode(string email);
    }
}
