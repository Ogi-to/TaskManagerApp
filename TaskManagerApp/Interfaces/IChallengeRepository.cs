using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IChallengeRepository
    {
        public List<Challenge> GetAll();

        public Challenge Get(Challenge item);

        public void JoinChallenge();

        public void CompleteChalenge();
        

    }
}
