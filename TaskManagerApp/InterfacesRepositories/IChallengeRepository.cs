using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IChallengeRepository
    {
        public Challenge Get(Challenge item);
        public List<Challenge> GetAll();
        public List<Challenge> GetChallengesByUser(User item);

        public void JoinChallenge(Challenge challenge, User user);

        public void CompleteChalenge(Challenge challenge, User user);
        

    }
}
