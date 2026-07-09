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
        public async Task<UserStats> GetAsync(int userId)
        {
            return await _context.UserStats.Where(us => us.UserId == userId).Include(us => us.User).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(UserStats item)
        {
            var userStatsToModify = await _context.UserStats.Where(us => us.UserId == item.UserId).FirstOrDefaultAsync();
            userStatsToModify.TasksCompleted = item.TasksCompleted;
            userStatsToModify.ChallengesCompleted = item.ChallengesCompleted;
            await _context.SaveChangesAsync();
        }
    }
}
