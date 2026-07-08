using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IRankRepository
    {
        public Task<Rank> GetAsync(Rank item);

        public Task<Rank> GetRankForPoints(int points);
        public Task<List<Rank>> GetAllAsync();


    }
}