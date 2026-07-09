using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class UserStatsService : IUserStatsService
    {
        private readonly IUserStatsRepository _repository;
        private readonly IUserRepository _userRepository;
        public UserStatsService(IUserStatsRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
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

        public async Task UpdateUserStatsByIdAsync(int userId)
        {
            //checks if the current stats of an user by id are different from the new 
            //and accordingly updates if necessary
            var currentStats = await _repository.GetAsync(userId);
            var user = await _userRepository.GetAsync(userId);
            if (user.Streak > currentStats.HighestStreak)
            {
                currentStats.HighestStreak = user.Streak;
            }
            if (user.Points > currentStats.TotalPoints)
            {
                currentStats.TotalPoints = user.Points;
            }

            await _repository.UpdateAsync(currentStats);
        }
    }
}