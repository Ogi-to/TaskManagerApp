using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class UserStatsService : IUserStatsService
    {
        private readonly UserStatsRepository _repository;
        public UserStatsService(UserStatsRepository repository)
        {
            _repository = repository;
        }
        public async Task<UserStats> ShowUserStatsByIdAsync(int userId)
        {
            var userStats = await _repository.GetAsync(userId);
            if (userStats == null)
            {
                throw new UserNotFoundException(userId);
            }
            return new UserStats
            {
                UserId = userId,
                User = userStats.User,
                HighestStreak = userStats.HighestStreak,
                TasksCompleted = userStats.TasksCompleted,
                ChallengesCompleted = userStats.ChallengesCompleted,
                TotalPoints = userStats.TotalPoints
            };
        }

        public async Task UpdateUserStatsByIdAsync(int userId, UserStats userStats)
        {
            //checks if the current stats of an user by id are different from the new 
            //and accordingly updates if necessary
            var currentStats = await _repository.GetAsync(userId);
            if (currentStats.HighestStreak == userStats.HighestStreak &&
                currentStats.TasksCompleted == userStats.TasksCompleted &&
                currentStats.ChallengesCompleted == userStats.ChallengesCompleted &&
                currentStats.TotalPoints == userStats.TotalPoints)
            {
                throw new Exception("No changes detected.");
            }
            await _repository.UpdateAsync(userStats);

        }
    }
}