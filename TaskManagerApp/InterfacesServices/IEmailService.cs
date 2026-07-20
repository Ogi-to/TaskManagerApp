using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IEmailService
    {
        public Task<bool> VerifyEmail(string email, string code);
        public Task SendVerificationCode(string email);
        public Task SendReminderEmail(string username, string email, TaskItem taskItem);
        public Task SendEmailForNewChallenges(string username, string email, List<Challenge> challenges);
        public Task DeleteCodes();
    }
}
