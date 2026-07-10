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
        public async Task CompleteChallengeAsync(Challenge challenge, User user)
        {
            var userChallenge = await _context.UsersChallenges.Where(uc => uc.ChallengeId == challenge.Id && uc.UserId == user.Id).Include(uc => uc.Challenge).FirstOrDefaultAsync();
            var challengeToComplete = userChallenge.Challenge;
            userChallenge.State = StateType.Completed;
            await _context.SaveChangesAsync();
        }


        public async Task<Challenge> GetAsync(int challengeId)
        {
            return await _context.Challenges.Where(c => c.Id == challengeId).FirstOrDefaultAsync();
        }

        public async Task<List<Challenge>> GetAllAsync()
        {
            return await _context.Challenges.Include(c => c.UsersChallenges).ThenInclude(uc => uc.User).ToListAsync();
        }
        public async Task<List<Challenge>> GetAllByCategoryAsync(int categoryId)
        {
            return await _context.Challenges.Where(c => c.CategoryId == categoryId).ToListAsync();
        }

        public async Task<List<Challenge>> GetAllActiveAsync()
        {
            return await _context.Challenges.Where(c => c.StartDate != null && c.EndDate != null).ToListAsync();
        }
        public async Task<List<Challenge>> GetAllByLevelAsync(int levelNumber)
        {
            return await _context.Challenges.Where(c => c.Level == levelNumber).ToListAsync();
        }
        public async Task<List<Challenge>> GetAllCompletedByUserAsync(int userId)
        {
            return await _context.UsersChallenges.Where(uc => uc.UserId == userId && uc.State == StateType.Completed).Select(uc => uc.Challenge).ToListAsync();
        }
        public async Task<List<Challenge>> GetChallengesByUserAsync(int userId)
        {
            return await _context.UsersChallenges.Where(uc => uc.UserId == userId).Select(uc => uc.Challenge).ToListAsync();
        }

        public async Task JoinChallengeAsync(Challenge challenge, User user)
        {
            UsersChallenges userChallenge = new UsersChallenges
            {
                UserId = user.Id,
                ChallengeId = challenge.Id,
                State = StateType.InProgress
            };
            await _context.UsersChallenges.AddAsync(userChallenge);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Challenge>> ChooseRandomChallenges(int numberOfChallengesToGet)
        {
            var random = new Random();

            var availableChallenges = await _context.Challenges
                .Where(c => c.StartDate == null && c.EndDate == null)
                .ToListAsync();

            var randomChallenges = availableChallenges
                .OrderBy(_ => random.Next())
                .Take(numberOfChallengesToGet)
                .ToList();

            foreach (var challenge in randomChallenges)
            {
                challenge.StartDate = DateOnly.FromDateTime(DateTime.Now);
                challenge.EndDate = DateOnly.FromDateTime(
                    DateTime.Now.AddDays(challenge.DurationDays));
            }

            await _context.SaveChangesAsync();

            return randomChallenges;
        }

        public async Task RemoveChallengesActivity(List<Challenge> challenges)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var expiredChallenges = challenges
                .Where(c => c.StartDate != null &&
                            c.EndDate != null &&
                            c.EndDate < today)
                .ToList();

            var expiredIds = expiredChallenges
                .Select(c => c.Id)
                .ToList();

            var userChallenges = await _context.UsersChallenges
                .Where(uc => expiredIds.Contains(uc.ChallengeId))
                .ToListAsync();

            _context.UsersChallenges.RemoveRange(userChallenges);

            foreach (var challenge in expiredChallenges)
            {
                challenge.StartDate = null;
                challenge.EndDate = null;
            }

            await _context.SaveChangesAsync();
        }
    }
}
