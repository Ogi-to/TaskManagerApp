using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IRankRepository
    {
        public Task<Rank> GetAsync(int rankId);
        public Task<List<Rank>> GetAllAsync();


    }
}
