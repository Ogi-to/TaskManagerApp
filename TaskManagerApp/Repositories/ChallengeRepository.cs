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
        public async Task CompleteChalengeAsync(Challenge challenge, User user)
        {
            var userChallenge = await _context.UsersChallenges.Where(uc => uc.ChallengeId == challenge.Id && uc.UserId == user.Id).Include(uc => uc.Challenge).FirstOrDefaultAsync();
            var challengeToComplete = userChallenge.Challenge;
            challengeToComplete.State = StateType.Completed;
            challengeToComplete.StateId = (int)StateType.Completed;
            await _context.SaveChangesAsync();
        }


        public async Task<Challenge> GetAsync(Challenge item)
        {
            return await _context.Challenges.Where(c => c.Id == item.Id).FirstOrDefaultAsync();
        }

        public async Task<List<Challenge>> GetAllAsync()
        {
            return await _context.Challenges.Include(c => c.UsersChallenges).ThenInclude(uc => uc.User).ToListAsync();
        }

        public async Task<List<Challenge>> GetChallengesByUserAsync(User item)
        {
            return await _context.UsersChallenges.Where(uc => uc.UserId == item.Id).Select(uc => uc.Challenge).ToListAsync();
        }

        public async Task JoinChallengeAsync(Challenge challenge, User user)
        {
            UsersChallenges userChallenge = new UsersChallenges
            {
                UserId = user.Id,
                ChallengeId = challenge.Id,
                StateId = (int)StateType.InProgress,
                State = StateType.InProgress
            };
            await _context.UsersChallenges.AddAsync(userChallenge);
            await _context.SaveChangesAsync();
        }
    }
}
