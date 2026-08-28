using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.InterfacesRepositories;

namespace TaskManagerApp.Repositories
{
    public class LoginAttemptRepository : ILoginAttemptRepository
    {

        private readonly TaskManagerDbContext _context;
        public LoginAttemptRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task<LoginAttempt> GetByIpAddressAsync(string ipAddress)
        {
            LoginAttempt loginAttempt = _context.LoginAttempts.Where(la => la.IpAddress == ipAddress).FirstOrDefault();
            return loginAttempt;
        }

        public async Task AddAsync(LoginAttempt loginAttempt)
        {
            await _context.LoginAttempts.AddAsync(loginAttempt);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(LoginAttempt loginAttempt)
        {
            _context.LoginAttempts.Update(loginAttempt);
            await _context.SaveChangesAsync();
        }
    }
}
