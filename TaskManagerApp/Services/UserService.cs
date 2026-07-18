using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesRepositories;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRankRepository _rankRepository;
        private readonly IEmailCodeRepository _emailCodeRepository;
        private readonly IEmailService _emailCodeService;
        private readonly HashPasswordService _hashPasswordService;
        private readonly IUserStatsService _userStatsService;
        private readonly ITaskItemRepository _taskItemRepository;

        public UserService(IUserRepository userRepository, IRankRepository rankRepository, IEmailCodeRepository emailCodeRepository, 
            IEmailService emailCodeService, HashPasswordService hashPasswordService, IUserStatsService userStatsService, ITaskItemRepository taskItemRepository)
        {
            _userRepository = userRepository;
            _rankRepository = rankRepository;
            _emailCodeRepository = emailCodeRepository;
            _emailCodeService = emailCodeService;
            _hashPasswordService = hashPasswordService;
            _userStatsService = userStatsService;
            _taskItemRepository = taskItemRepository;
        }
        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Streak = user.Streak,
                Points = user.Points,
                RankId = user.RankId,
                CreatedAt = user.CreatedAt,
                LastActive = user.LastActive,
                UserCode = user.UserCode
            };

        }

        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var testUser = await _userRepository.GetByUsernameAsync(registerUserDto.Username);
            if (testUser != null)
            {
                throw new UserNameAlreadyExistsException();
            }
            testUser = await _userRepository.GetByEmailAsync(registerUserDto.Email);
            if (testUser != null)
            {
                throw new UserEmailAlreadyExistsException();
            }

            var user = new User
            {
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                PasswordHash = _hashPasswordService.HashPassword(registerUserDto.Password),
                UserCode = Random.Shared.Next(10000000, 99999999).ToString(),
                IsEmailVerified = false,
                LastActive = null,
            }; ;

            

            await _userRepository.CreateAccountAsync(user);
            await _userStatsService.CreateUserStatsAsync(user.Id);
            await _emailCodeService.SendVerificationCode(user.Email);
        }

        public async Task<UserDto> LogInUser(LoginUserDto loginUserDto)
        {
            var testUser = await _userRepository.GetByEmailAsync(loginUserDto.Email);
            if (testUser == null)
            {
                throw new EmailorPasswordNotFoundException();
            }

            if (testUser.IsEmailVerified == false)
            {
                throw new EmailNotVerifiedException();
            }

            var isPasswordValid = _hashPasswordService.VerifyPassword(loginUserDto.Password, testUser.PasswordHash);

            if (isPasswordValid == false)
            {
                throw new EmailorPasswordNotFoundException();
            }

            return new UserDto
            {
                Id = testUser.Id,
                Username = testUser.Username,
                Email = testUser.Email,
                Streak = testUser.Streak,
                Points = testUser.Points,
                RankId = testUser.RankId,
                CreatedAt = testUser.CreatedAt,
                LastActive = testUser.LastActive,
                UserCode = testUser.UserCode
            };
        }

        public async Task DeleteAccount(int userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            await _userRepository.DeleteAccountAsync(user.Id);
        }

        public async Task UpdateStreak(User user)
        {
          
            var today = DateTime.UtcNow.Date;
            if (user.LastActive?.Date == today)
            {
                return;
            }

            if (user.LastActive?.Date == today.AddDays(-1))
            {
                user.Streak += 1;
            }
            else
            {
                user.Streak = 1;
            }

            user.LastActive = DateTime.UtcNow;
        }

        public async Task UpdateRank(User user)
        {
            
            var newRank = await _rankRepository.GetRankForPoints(user.Points);
            if (newRank == null)
            {
                throw new Exception("No rank found for the given points.");
            }
            user.RankId = newRank.Id;
        }

        public async Task UpdatePoints(int userId, int points)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            user.Points += points;


            await UpdateRank(user);
            await UpdateStreak(user);
            await _userRepository.UpdateUserInfoAsync(user);
        }

        public async Task UpdateReminderSettings(int userId, ReminderSettingsDto reminderSettingsDto)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            user.ReminderStartBefore = reminderSettingsDto.ReminderStartBefore;
            user.ReminderInterval = reminderSettingsDto.ReminderInterval;
            await _userRepository.UpdateAccountInfoAsync(user);
        }

        public async Task SendReminderForTasksEmail()
        {
            List<TaskItem> tasks = await _taskItemRepository.GetAllAboutToStartAsync();
            foreach (var task in tasks)
            {
                UserDto user = await GetUserById(task.UserId);
                if (task.LastSendReminder == null || task.LastSendReminder.Value.AddMinutes(user.ReminderInterval) <= DateTime.UtcNow)
                {
                    await _emailCodeService.SendReminderEmail(user, task);
                    task.LastSendReminder = DateTime.UtcNow;
                }
            }
            await _taskItemRepository.SaveChanges();


        }


        public async Task VerifyEmail(VerifyEmailDto verifyEmailDto)
        {
            var isVerified = await _emailCodeService.VerifyEmail(verifyEmailDto.Email, verifyEmailDto.Code);
            if (!isVerified)
            {
                throw new InvalidVerificationCodeException();
            }
            var user = await _userRepository.GetByEmailAsync(verifyEmailDto.Email);
            if (user == null)
            {
                throw new EmailorPasswordNotFoundException();
            }
            user.IsEmailVerified = true;
            await _userRepository.UpdateAccountInfoAsync(user);
        }
    }
}