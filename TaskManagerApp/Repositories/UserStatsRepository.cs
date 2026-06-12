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
        public UserStats Get(UserStats item)
        {
            return _context.UserStats.Where(us => us.UserId == item.UserId).Include(us => us.User).FirstOrDefault();
        }

        public void Update(UserStats item)
        {
            UserStats userStatsToModify = _context.UserStats.Where(us => us.UserId == item.UserId).FirstOrDefault();
            userStatsToModify.HighestStreak = item.HighestStreak;
            userStatsToModify.TasksCompleted = item.TasksCompleted;
            userStatsToModify.ChallengesCompleted = item.ChallengesCompleted;
            userStatsToModify.TotalPoints = item.TotalPoints;
            _context.SaveChanges();
        }
    }
}
