using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesRepositories
{
    public interface ILoginAttemptRepository
    {
        public Task<LoginAttempt> GetByIpAddressAsync(string ipAddress);
        public Task AddAsync(LoginAttempt loginAttempt);
        public  Task UpdateAsync(LoginAttempt loginAttempt);
    }
}
