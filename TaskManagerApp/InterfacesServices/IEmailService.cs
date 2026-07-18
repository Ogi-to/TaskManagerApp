using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IEmailService
    {
        public Task<bool> VerifyEmail(string email, string code);
        public Task SendVerificationCode(string email);
        public Task SendReminderEmail(UserDto userdto, TaskItem taskItem);
        public Task SendEmailForNewChallenges(UserDto user, List<Challenge> challenges);
        public Task DeleteCodes();
    }
}
