using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IChallengeRepository
    {
        public Task<Challenge> GetAsync(Challenge item);
        public Task<List<Challenge>> GetAllAsync();
        public Task<List<Challenge>> GetChallengesByUserAsync(User item);

        public Task JoinChallengeAsync(Challenge challenge, User user);

        public Task CompleteChalengeAsync(Challenge challenge, User user);


    }
}
