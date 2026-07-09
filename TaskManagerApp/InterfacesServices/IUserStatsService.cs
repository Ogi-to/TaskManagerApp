using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesServices
{
    public interface IUserStatsService
    {
        public Task<UserStats> ShowUserStatsByIdAsync(int userId);
        public Task UpdateUserStatsByIdAsync(int userId);
    }
}
