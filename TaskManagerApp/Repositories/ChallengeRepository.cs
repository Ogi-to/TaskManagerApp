using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private TaskManagerDbContext _context;
        public ChallengeRepository(TaskManagerDbContext context)
        {
            _context = context;
        }
        public void CompleteChalenge(Challenge challenge, User user)
        {
            Challenge challengeToComplete = _context.UsersChallenges.Where(uc => uc.ChallengeId == challenge.Id && uc.UserId == user.Id).Include(uc => uc.Challenge).FirstOrDefault().Challenge;
            challengeToComplete.State = StateType.Completed;
            challengeToComplete.StateId = (int)StateType.Completed;
            _context.SaveChanges();
        }
        

        public Challenge Get(Challenge item)
        {
            return _context.Challenges.Where(c => c.Id == item.Id).FirstOrDefault();
        }

        public List<Challenge> GetAll()
        {
            return _context.Challenges.Include(c => c.Users).ToList();
        }

        public List<Challenge> GetChallengesByUser(User item)
        {
           return _context.UsersChallenges.Where(uc => uc.UserId == item.Id).Select(uc => uc.Challenge).ToList();
        }

        public void JoinChallenge(Challenge challenge, User user)
        { 
            UsersChallenges userChallenge = new UsersChallenges 
            {   UserId = user.Id, 
                ChallengeId = challenge.Id, 
                StateId = (int)StateType.InProgress, 
                State = StateType.InProgress 
            };
            _context.UsersChallenges.Add(userChallenge);
            _context.SaveChanges();
        }
    }
}
