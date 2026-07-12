using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesServices
{
    public interface IUserStatsService
    {
        public Task CreateUserStatsAsync(int userId);
        public Task<UserStats> ShowUserStatsByIdAsync(int userId);
        public Task UpdateUserStatsAsync(UserStats userStats);
    }
}
