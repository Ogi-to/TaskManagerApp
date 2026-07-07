using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IChallengeRepository
    {
        public Task<Challenge> GetAsync(int challengeId);
        public Task<List<Challenge>> GetAllAsync();
        public Task<List<Challenge>> ShowAllByCategoryAsync(int categoryId);
        public Task<List<Challenge>> ShowAllByLevelAsync(int levelNumber);
        public Task<List<Challenge>> ShowAllCompletedByUserAsync(int userId);
        public Task<List<Challenge>> GetChallengesByUserAsync(int userId);

        public Task JoinChallengeAsync(Challenge challenge, User user);
        public Task CompleteChallengeAsync(Challenge challenge, User user);


    }
}
