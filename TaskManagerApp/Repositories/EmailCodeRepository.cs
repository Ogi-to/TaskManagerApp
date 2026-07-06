using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.InterfacesRepositories;

namespace TaskManagerApp.Repositories
{
    public class EmailCodeRepository : IEmailCodeRepository
    {
        readonly TaskManagerDbContext _dbContext;
        public EmailCodeRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(EmailCode emailCode)
        {
            await _dbContext.AddAsync(emailCode);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteExpiredCodesAsync()
        {
            var expiredCodes = await _dbContext.EmailCodes.Where(x => x.ExpirationTime <= DateTime.UtcNow || x.IsUsed).ToListAsync();
            _dbContext.EmailCodes.RemoveRange(expiredCodes);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<EmailCode?> GetValidCodeAsync(string email, string code)
        {
            return await _dbContext.EmailCodes.FirstOrDefaultAsync(x => x.Email == email && x.Code == code && x.ExpirationTime > DateTime.UtcNow && !x.IsUsed);
        }

        public async Task MarkAsUsedAsync(EmailCode emailCode)
        {
            emailCode.IsUsed = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}
