using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IChallengeService
    {
        public Task<ChallengeDto> ShowAsync(int challengeId);
        public Task<List<ChallengeDto>> ShowAllAsync();
        public Task<List<ChallengeDto>> ShowAllByCategoryAsync(int categoryId);
        public Task<List<ChallengeDto>> ShowAllByLevelAsync(int levelNumber);
        public Task<List<ChallengeDto>> ShowAllCompletedByUserAsync(int userId);
        public Task<List<ChallengeDto>> ShowAllByUserAsync(int userId);

        public Task JoinChallengeAsync(int challengeId, int userId);
        public Task CompleteChallengeAsync(int challengeId, int userId);
        public Task<List<ChallengeDto>> GetAllActive();
        public Task RemoveChallengesActivity();
        public Task ChooseRandomChallenges();
    }
}
