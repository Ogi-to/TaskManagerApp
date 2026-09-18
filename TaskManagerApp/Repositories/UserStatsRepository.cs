using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class UserStatsRepository : IUserStatsRepository
    {
        private TaskManagerDbContext _context;
        public UserStatsRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task CreateUserStats(UserStats userStats)
        {
            await _context.UserStats.AddAsync(userStats);
            await _context.SaveChangesAsync();
        }
        public async Task<UserStats> GetAsync(int userId)
        {
            return await _context.UserStats.Where(us => us.UserId == userId).Include(us => us.User).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(UserStats item)
        {
            var userStatsToModify = await _context.UserStats
                .Where(us => us.UserId == item.UserId)
                .FirstOrDefaultAsync();

            if (userStatsToModify == null)
            {
                throw new Exception(
                    $"User stats for user with ID {item.UserId} not found.");
            }

            // TasksCompleted е исторически брояч.
            // Никога не го намаляваме.
            if (item.TasksCompleted > userStatsToModify.TasksCompleted)
            {
                userStatsToModify.TasksCompleted = item.TasksCompleted;
            }

            if (item.ChallengesCompleted >
                userStatsToModify.ChallengesCompleted)
            {
                userStatsToModify.ChallengesCompleted =
                    item.ChallengesCompleted;
            }

            await _context.SaveChangesAsync();
        }
    }
}
