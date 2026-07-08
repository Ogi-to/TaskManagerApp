using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class RankService : IRankService
    {
        private readonly IRankRepository _repository;
        public RankService(IRankRepository repository)
        {
            _repository = repository;

        }
        public async Task<List<Rank>> ShowAll()
        {
            var ranks = await _repository.GetAllAsync();
            if (ranks == null || ranks.Count ==0)
            {
                throw new Exception("There are no ranks.");
            }
            return ranks;
        }

        public async Task<Rank> ShowAsync(int rankId)
        {
            var existingRank = await _repository.GetAsync(rankId);
            if (existingRank == null)
            {
                throw new Exception($"Rank with ID {rankId} was not found.");
            }

            return existingRank;
        }
    }
}
