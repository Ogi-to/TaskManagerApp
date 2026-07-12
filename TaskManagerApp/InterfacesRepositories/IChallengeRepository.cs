using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IChallengeRepository
    {
        public Task<Challenge> GetAsync(int challengeId);
        public Task<List<Challenge>> GetAllAsync();
        public Task<List<Challenge>> GetAllByCategoryAsync(int categoryId);
        public Task<List<Challenge>> GetAllByLevelAsync(int levelNumber);
        public Task<List<Challenge>> GetAllCompletedByUserAsync(int userId);
        public Task<List<Challenge>> GetChallengesByUserAsync(int userId);

        public Task JoinChallengeAsync(int challengeId, int userId);
        public Task CompleteChallengeAsync(int challengeId, int userId);
        public Task<List<Challenge>> ChooseRandomChallenges(int numberOfChallengesToGet);

        public Task RemoveChallengesActivity(List<Challenge> challenges);
        public Task<List<Challenge>> GetAllActiveAsync();
    }
}
