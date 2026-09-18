using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
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
                throw new RankNotFoundException();
            }
            return ranks;
        }

        public async Task<Rank> ShowAsync(int rankId)
        {
            var existingRank = await _repository.GetAsync(rankId);
            if (existingRank == null)
            {
                throw new RankNotFoundException();
            }

            return existingRank;
        }
    }
}
