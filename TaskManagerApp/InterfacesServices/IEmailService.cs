using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesServices
{
    public interface IEmailService
    {
        public Task<bool> VerifyEmail(string email, string code);
        public Task SendVerificationCode(string email);
        public Task SendReminderEmail(string email, User user, TaskItem taskItem);
        public Task DeleteCodes();
    }
}
