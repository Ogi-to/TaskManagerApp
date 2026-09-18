//using Org.BouncyCastle.Asn1.Cmp; nqmam predstawa otkyde doide
using TaskManagerApp.Data.Models;
using TaskManagerApp.DtoMappers;
using TaskManagerApp.DTOS;
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
        private readonly IEmailService _emailService;
        public ChallengeService(IChallengeRepository challengeRepository, IUserRepository userRepository, ICategoryRepository categoryRepository,
            IUserStatsService userStatsService, IUserService userService, IEmailService emailService)
        {
            _challengeRepository = challengeRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _userStatsService = userStatsService;
            _userService = userService;
            _emailService = emailService;
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
                throw new ChallengeNotFoundException();
            }
            if (existingUser.UsersChallenges.Where(uc => uc.ChallengeId == challengeId && uc.State == StateType.Completed).Any())
            {
                throw new ChallengeAlreadyCompletedException();
            }
            if (!existingUser.UsersChallenges.Where(uc => uc.ChallengeId == challengeId).Any())
            {
                throw new UserHasNotJoinedTheChallengeException();
            }
            await _userService.UpdatePoints(existingUser.Id, existingChallenge.Points);

            var userStats = await _userStatsService.ShowUserStatsByIdAsync(userId);
            userStats.ChallengesCompleted += 1;
            await _userStatsService.UpdateUserStatsAsync(userStats);


            await _challengeRepository.CompleteChallengeAsync(existingChallenge.Id, existingUser.Id);
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
                throw new ChallengeNotFoundException();
            }
            if (existingUser.UsersChallenges.Where(uc => uc.ChallengeId == challengeId).Any())
            {
                throw new ChallengeAlreadyCompletedException();
            }
            if (existingChallenge.StartDate == default || existingChallenge.EndDate == default)
            {
                throw new ChallengeHasNotStartedException();
            }

            await _challengeRepository.JoinChallengeAsync(existingChallenge.Id, existingUser.Id);

        }

        public async Task<List<ChallengeDto>> ShowAllAsync()
        {
            var challenges = await _challengeRepository.GetAllAsync();
            return challenges.Select(c => new ChallengeDto
            {
                Id = c.Id,
                Title = c.Title,
                DurationDays = c.DurationDays,
                Level = c.Level,
                CategoryId = c.CategoryId,
                Trophy = c.Trophy,
                Points = c.Points,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate
            }).ToList();
        }

        public async Task<List<ChallengeDto>> ShowAllByCategoryAsync(int categoryId)
        {
            var existingCategory = await _categoryRepository.GetAsync(categoryId);
            if (existingCategory == null)
            {
                throw new CategoryNotFoundException();
            }
            var sortedChallenges = await _challengeRepository.GetAllByCategoryAsync(categoryId);
            if (sortedChallenges == null || sortedChallenges.Count == 0)
            {
                throw new NoChallengesFoundForThisCategoryException();
            }
            return sortedChallenges.Select(c => new ChallengeDto
            {
                Id = c.Id,
                Title = c.Title,
                DurationDays = c.DurationDays,
                Level = c.Level,
                CategoryId = c.CategoryId,
                Trophy = c.Trophy,
                Points = c.Points,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate
            }).ToList();
        }

        public async Task<List<ChallengeDto>> ShowAllByLevelAsync(int levelNumber)
        {
            var sortedChallenges = await _challengeRepository.GetAllByLevelAsync(levelNumber);
            if (sortedChallenges == null || sortedChallenges.Count == 0)
            {
                throw new ThereAreNoChallengesForThisCategoryException();
            }
            return sortedChallenges.Select(c => new ChallengeDto
            {
                Id = c.Id,
                Title = c.Title,
                DurationDays = c.DurationDays,
                Level = c.Level,
                CategoryId = c.CategoryId,
                Trophy = c.Trophy,
                Points = c.Points,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate
            }).ToList();
        }

        public async Task<List<ChallengeDto>> ShowAllByUserAsync(int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            if (existingUser.UsersChallenges == null || !existingUser.UsersChallenges.Any())
            {
                throw new UserHasntJoinedAnyChallengesException();
            }

            var challenges = await _challengeRepository.GetChallengesByUserAsync(userId);

            return challenges.Select(c => new ChallengeDto
            {
                Id = c.Id,
                Title = c.Title,
                DurationDays = c.DurationDays,
                Level = c.Level,
                CategoryId = c.CategoryId,
                Trophy = c.Trophy,
                Points = c.Points,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate
            }).ToList();
        }

        public async Task<List<ChallengeDto>> ShowAllCompletedByUserAsync(int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            if (existingUser.UsersChallenges == null || !existingUser.UsersChallenges.Any())
            {
                throw new UserHasntJoinedAnyChallengesException();
            }
            if (!existingUser.UsersChallenges.Where(uc => uc.State == StateType.Completed).Any())
            {
                throw new UserHasntCompletedAnyChallengesException();
            }

            var completedChallenges = await _challengeRepository.GetAllCompletedByUserAsync(userId);
            return completedChallenges.Select(c => new ChallengeDto
            {
                Id = c.Id,
                Title = c.Title,
                DurationDays = c.DurationDays,
                Level = c.Level,
                CategoryId = c.CategoryId,
                Trophy = c.Trophy,
                Points = c.Points,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate
            }).ToList();
        }

        public async Task<ChallengeDto> ShowAsync(int challengeId)
        {
            var existingChallenge = await _challengeRepository.GetAsync(challengeId);
            if (existingChallenge == null)
            {
                throw new ChallengeNotFoundException();
            }

            return new ChallengeDto
            {
                Id = existingChallenge.Id,
                Title = existingChallenge.Title,
                DurationDays = existingChallenge.DurationDays,
                Level = existingChallenge.Level,
                CategoryId = existingChallenge.CategoryId,
                Trophy = existingChallenge.Trophy,
                Points = existingChallenge.Points,
                Description = existingChallenge.Description,
                StartDate = existingChallenge.StartDate,
                EndDate = existingChallenge.EndDate
            };
        }

        public async Task ChooseRandomChallenges()
        {
            int possibleNumberOfActiveChallengesAtOneTime = 3;
            var currentlyActiveChallenges = await _challengeRepository.GetAllActiveAsync();

            if (currentlyActiveChallenges.Count < possibleNumberOfActiveChallengesAtOneTime)
            {
                await _challengeRepository.ChooseRandomChallenges(possibleNumberOfActiveChallengesAtOneTime - currentlyActiveChallenges.Count);
                currentlyActiveChallenges = await _challengeRepository.GetAllActiveAsync();
                List<User> users = await _userRepository.GetAllUsersAsync();
                foreach (User user in users)
                {
                    await _emailService.SendEmailForNewChallenges(user.Username, user.Email, currentlyActiveChallenges);
                }
            }


        }

        public async Task<List<ChallengeDto>> GetAllActive()
        {
            var activeChallenges = await _challengeRepository.GetAllActiveAsync();
            List<ChallengeDto> challengeDtos = new List<ChallengeDto>();
            foreach (var challenge in activeChallenges)
            {
                challengeDtos.Add(challenge.ToDto());
            }
            return challengeDtos;
        }

        public async Task RemoveChallengesActivity()
        {
            var currentlyActiveChallenges = await _challengeRepository.GetAllActiveAsync();

            await _challengeRepository.RemoveChallengesActivity(currentlyActiveChallenges);
        }
    }
}
