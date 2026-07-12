using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserStatsRepository
    {
        public Task CreateUserStats(UserStats userStats);
        public Task<UserStats> GetAsync(int userId);

        public Task UpdateAsync(UserStats item);


    }
}
