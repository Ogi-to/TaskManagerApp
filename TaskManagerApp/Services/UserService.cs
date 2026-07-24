using TaskManagerApp.Data.Models;
using TaskManagerApp.DtoMappers;
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
        private readonly IEmailService _emailCodeService;
        private readonly HashPasswordService _hashPasswordService;
        private readonly IUserStatsService _userStatsService;
        private readonly ITaskItemRepository _taskItemRepository;

        public UserService(IUserRepository userRepository, IRankRepository rankRepository, ITaskItemRepository taskItemRepository,
            IEmailService emailCodeService, HashPasswordService hashPasswordService, IUserStatsService userStatsService)
        {
            _userRepository = userRepository;
            _rankRepository = rankRepository;
            _emailCodeService = emailCodeService;
            _hashPasswordService = hashPasswordService;
            _userStatsService = userStatsService;
            _taskItemRepository = taskItemRepository;
           
        }
        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetAsync(id);

            if (user == null)
                throw new UserNotFoundException(id);

            return user.ToDto();

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
            await _emailCodeService.SendVerificationCode(testUser.Email);
            var isVerified = await _emailCodeService.VerifyEmail(testUser.Email, loginUserDto.Code);
            if (!isVerified)
            {
                throw new InvalidVerificationCodeException();
            }

            return testUser.ToDto();
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

        public async Task UpdateStreak(UpdateUserDto user)
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

        public async Task UpdateRank(UpdateUserDto user)
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
            User user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            user.Points += points;
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                Points = user.Points,
                RankId = user.RankId,
                LastActive = user.LastActive,
                Streak = user.Streak
            };

            await UpdateRank(updateUserDto);
            await UpdateStreak(updateUserDto);
            await _userRepository.UpdateUserInfoAsync(updateUserDto, user.Id);
        }

        public async Task UpdateReminderSettings(ReminderSettingsDto reminderSettingsDto)
        {
            var user = await _userRepository.GetAsync(reminderSettingsDto.Id);
            if (user == null)
            {
                throw new UserNotFoundException(reminderSettingsDto.Id);
            }
            ReminderSettingsDto reminderSettings = new ReminderSettingsDto
            {
                Id = reminderSettingsDto.Id,
                ReminderInterval = reminderSettingsDto.ReminderInterval,
                ReminderStartBefore = reminderSettingsDto.ReminderStartBefore,
            };

            reminderSettings.ReminderStartBefore = reminderSettingsDto.ReminderStartBefore;
            reminderSettings.ReminderInterval = reminderSettingsDto.ReminderInterval;
            await _userRepository.UpdateUserReminders(reminderSettings, user.Id);
        }

        public async Task SendReminderForTasksEmail()
        {
            List<TaskItem> tasks = await _taskItemRepository.GetAllAboutToStartAsync();
            Console.WriteLine(tasks.Count);
            foreach (var task in tasks)
            {

                User user = task.User;
                if (task.LastSendReminder == default || task.LastSendReminder.Value.AddMinutes(user.ReminderInterval) <= DateTime.UtcNow)
                {
                    await _emailCodeService.SendReminderTaskEmail(user.Username, user.Email, task);
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
            UpdateAccountDto updateAccountDto = new UpdateAccountDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Password = user.PasswordHash,
                IsEmailVerified = user.IsEmailVerified,
            };
            
            await _userRepository.UpdateAccountInfoAsync(updateAccountDto, user.Id);
        }

        public async Task ReSendVerificationCode(string email)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new EmailorPasswordNotFoundException();
            }
            await _emailCodeService.SendVerificationCode(user.Email);
        }

        public async Task SendReminderForStreakEmail()
        {
            var today = DateTime.UtcNow.Date;
            List<User> users = await _userRepository.GetUsersWithStreaksAboutToEndAsync();
            foreach (var user in users)
            {
                if (user.LastActive > today.AddDays(-1))
                {
                    await _emailCodeService.SendReminderForStreakEmail(user);
                }
               
            }

        }
    }
}