using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesRepositories
{
    public interface IEmailCodeRepository
    {
        Task AddAsync(EmailCode emailCode);
        Task<EmailCode?> GetValidCodeAsync(string email, string code);
        Task MarkAsUsedAsync(EmailCode emailCode);
        Task DeleteExpiredCodesAsync();
    }
}
