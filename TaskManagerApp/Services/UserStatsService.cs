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

        public async Task CreateUserStatsAsync(int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            var existingStats = await _repository.GetAsync(userId);
            if (existingStats != null)
            {
                throw new UserStatsAlreadyExistException();
            }
            var userStats = new UserStats
            {
                UserId = existingUser.Id,
                HighestStreak = 0,
                TasksCompleted = 0,
                ChallengesCompleted = 0,
                TotalPoints = 0
            };
            await _repository.CreateUserStats(userStats);
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

        public async Task UpdateUserStatsAsync(UserStats userStats)
        {
            //checks if the current stats of an user by id are different from the new 
            //and accordingly updates if necessary
            var currentStats = await _repository.GetAsync(userStats.UserId);
            if (currentStats == null)
            {
                throw new UserNotFoundException(userStats.UserId);
            }
            var user = await _userRepository.GetAsync(userStats.UserId);
            if (user == null) 
            {
                throw new UserNotFoundException(userStats.UserId);
            }
            if (user.Streak > currentStats.HighestStreak)
            {
                currentStats.HighestStreak = user.Streak;
            }
            if (user.Points > currentStats.TotalPoints)
            {
                currentStats.TotalPoints = user.Points;
            }
            currentStats.TasksCompleted = userStats.TasksCompleted;
            currentStats.ChallengesCompleted = userStats.ChallengesCompleted;

            await _repository.UpdateAsync(currentStats);
        }
    }
}