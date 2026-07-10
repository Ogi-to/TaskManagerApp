using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesServices
{
    public interface IChallengeService
    {
        public Task<Challenge> ShowAsync(int challengeId);
        public Task<List<Challenge>> ShowAllAsync();
        public Task<List<Challenge>> ShowAllByCategoryAsync(int categoryId);
        public Task<List<Challenge>> ShowAllByLevelAsync(int levelNumber);
        public Task<List<Challenge>> ShowAllCompletedByUserAsync(int userId);
        public Task<List<Challenge>> ShowAllByUserAsync(int userId);

        public Task JoinChallengeAsync(int challengeId, int userId);
        public Task CompleteChallengeAsync(int challengeId, int userId);
        public Task RemoveChallengesActivity();
        public Task ChooseRandomChallenges();
    }
}
