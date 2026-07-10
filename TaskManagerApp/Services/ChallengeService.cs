//using Org.BouncyCastle.Asn1.Cmp; nqmam predstawa otkyde doide
using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class ChallengeService : IChallengeService
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserStatsService _userStatsService;
        private readonly IUserService _userService;
        public ChallengeService(IChallengeRepository challengeRepository, IUserRepository userRepository, ICategoryRepository categoryRepository,
            IUserStatsService userStatsService, IUserService userService)
        {
            _challengeRepository = challengeRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _userStatsService = userStatsService;
            _userService = userService;
        }
        public async Task CompleteChallengeAsync(int challengeId, int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            var existingChallenge = await _challengeRepository.GetAsync(challengeId);
            if (existingChallenge == null)
            {
                throw new Exception($"Challenge with ID {challengeId} doesn't exist.");
            }
            if (existingUser.UsersChallenges.Where(uc => uc.ChallengeId == challengeId && uc.State == StateType.Completed).Any())
            {
                throw new Exception($"User with ID {userId} has already completed the challenge with ID {challengeId}.");
            }
            if (!existingUser.UsersChallenges.Where(uc => uc.ChallengeId == challengeId).Any())
            {
                throw new Exception($"User with ID {userId} hasn't joined the challenge with ID {challengeId}.");
            }
            existingUser.Points += existingChallenge.Points;
            await _userService.UpdatePoints(existingUser);

            var userStats = await _userStatsService.ShowUserStatsByIdAsync(userId);
            userStats.ChallengesCompleted += 1;
            await _userStatsService.UpdateUserStatsByIdAsync(userStats.UserId);


            await _challengeRepository.CompleteChallengeAsync(existingChallenge, existingUser);
        }

        public async Task JoinChallengeAsync(int challengeId, int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            var existingChallenge = await _challengeRepository.GetAsync(challengeId);
            if (existingChallenge == null)
            {
                throw new Exception($"Challenge with ID {challengeId} doesn't exist.");
            }
            if (existingUser.UsersChallenges.Where(uc => uc.ChallengeId == challengeId).Any())
            {
                throw new Exception($"User with ID {userId} has already joined the challenge with ID {challengeId}.");
            }

            await _challengeRepository.JoinChallengeAsync(existingChallenge, existingUser);

        }

        public async Task<List<Challenge>> ShowAllAsync()
        {
            return await _challengeRepository.GetAllAsync();
        }

        public async Task<List<Challenge>> ShowAllByCategoryAsync(int categoryId)
        {
            var existingCategory = await _categoryRepository.GetAsync(categoryId);
            if (existingCategory == null)
            {
                throw new Exception($"Category with ID {categoryId} doesn't exist.");
            }
            var sortedChallenges = await _challengeRepository.GetAllByCategoryAsync(categoryId);
            if (sortedChallenges == null || sortedChallenges.Count == 0)
            {
                throw new Exception("There are no challenges from this category.");
            }
            return sortedChallenges;
        }

        public async Task<List<Challenge>> ShowAllByLevelAsync(int levelNumber)
        {
            var sortedChallenges = await _challengeRepository.GetAllByLevelAsync(levelNumber);
            if (sortedChallenges == null || sortedChallenges.Count == 0)
            {
                throw new Exception("There are no challenges with the specified level number.");
            }
            return sortedChallenges;
        }

        public async Task<List<Challenge>> ShowAllByUserAsync(int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            if (existingUser.UsersChallenges == null || !existingUser.UsersChallenges.Any())
            {
                throw new Exception($"User with ID {userId} has not joined any challenges.");
            }

            return await _challengeRepository.GetChallengesByUserAsync(userId);
        }

        public async Task<List<Challenge>> ShowAllCompletedByUserAsync(int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            if (existingUser.UsersChallenges == null || !existingUser.UsersChallenges.Any())
            {
                throw new Exception($"User with ID {userId} has not joined any challenges.");
            }
            if (!existingUser.UsersChallenges.Where(uc => uc.State == StateType.Completed).Any())
            {
                throw new Exception($"User with ID {userId} has not completed any challenges.");
            }

            return await _challengeRepository.GetAllCompletedByUserAsync(userId);

        }

        public async Task<Challenge> ShowAsync(int challengeId)
        {
            var existingChallenge = await _challengeRepository.GetAsync(challengeId);
            if (existingChallenge == null)
            {
                throw new Exception($"Challenge with ID {challengeId} not found.");
            }

            return existingChallenge;
        }

        public async Task ChooseRandomChallenges()
        {
            int possibleNumberOfActiveChallengesAtOneTime = 3;
            var currentlyActiveChallenges = await _challengeRepository.GetAllActiveAsync();

            if (currentlyActiveChallenges.Count < possibleNumberOfActiveChallengesAtOneTime)
            {
                await _challengeRepository.ChooseRandomChallenges(possibleNumberOfActiveChallengesAtOneTime - currentlyActiveChallenges.Count);
            }


        }

        public async Task RemoveChallengesActivity()
        {
            var currentlyActiveChallenges = await _challengeRepository.GetAllActiveAsync();

            await _challengeRepository.RemoveChallengesActivity(currentlyActiveChallenges);
        }
    }
}
