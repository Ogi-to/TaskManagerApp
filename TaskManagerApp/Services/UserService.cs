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
        private readonly IEmailCodeService _emailCodeService;
        private readonly HashPasswordService _hashPasswordService;

        public UserService(IUserRepository userRepository, IRankRepository rankRepository, IEmailCodeRepository emailCodeRepository, IEmailCodeService emailCodeService, HashPasswordService hashPasswordService)
        {
            _userRepository = userRepository;
            _rankRepository = rankRepository;
            _emailCodeRepository = emailCodeRepository;
            _emailCodeService = emailCodeService;
            _hashPasswordService = hashPasswordService;
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
            var testUser = _userRepository.GetByUsername(registerUserDto.Username);
            if (testUser != null)
            {
                throw new UserNameAlreadyExistsException();
            }
            testUser = _userRepository.GetByEmail(registerUserDto.Email);
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
                RankId = registerUserDto.RankId,
                IsEmailVerified = false

            };

            Console.WriteLine("Before saving user");
            _userRepository.CreateAccount(user);
            Console.WriteLine("After saving user");

            await _emailCodeService.SendVerificationCode(user.Email);
        }

        public async Task<UserDto> LogInUser(LoginUserDto loginUserDto)
        {
            var testUser = _userRepository.GetByEmail(loginUserDto.Email);
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

        public async Task DeleteAccount(User item)
        {
            var user = await _userRepository.GetAsync(item.Id);
            if (user == null)
            {
                throw new UserNotFoundException(item.Id);
            }
            _userRepository.DeleteAccount(user.Id);
        }

        public async Task<UpdateUserDto> UpdateUserInfo(User item)
        {
            var user = await _userRepository.GetAsync(item.Id);

            if (user == null)
            {
                throw new UserNotFoundException(item.Id);
            }


            user.Streak = item.Streak;
            user.Points = item.Points;
            user.RankId = item.RankId;
            user.LastActive = item.LastActive;

            await _userRepository.UpdateUserInfo(user);
            return new UpdateUserDto
            {
                Streak = item.Streak,
                Points = item.Points,
                RankId = item.RankId,
                LastActive = item.LastActive
            };
        }

        public async Task UpdateStreak(User item)
        {
            var user = await _userRepository.GetAsync(item.Id);
            var today = DateTime.UtcNow.Date;
            if (user == null)
            {
                throw new UserNotFoundException(item.Id);
            }

            if (user.LastActive.Date == today)
            {
                return;
            }

            if (user.LastActive.Date == today.AddDays(-1))
            {
                user.Streak += 1;
            }
            else
            {
                user.Streak = 1;
            }

            user.LastActive = DateTime.Today;
            await UpdateUserInfo(user);
        }

        public async Task UpdateRank(User item)
        {
            var user = await _userRepository.GetAsync(item.Id);
            if (user == null)
            {
                throw new UserNotFoundException(item.Id);
            }

            var newRank = await _rankRepository.GetRankForPoints(user.Points);
            user.RankId = newRank.Id;
            await UpdateUserInfo(user);
        }

        public async Task UpdatePoints(User item)
        {
            var user = await _userRepository.GetAsync(item.Id);
            if (user == null)
            {
                throw new UserNotFoundException(item.Id);
            }
            user.Points = item.Points;
            await UpdateUserInfo(user);
        }


        public async Task<bool> VerifyEmail(string email, string code)
        {
            var isVerified = await _emailCodeService.VerifyEmail(email, code);
            if (!isVerified)
            {
                throw new InvalidVerificationCodeException();
            }
            var user = _userRepository.GetByEmail(email);
            if (user == null)
            {
                throw new EmailorPasswordNotFoundException();
            }
            user.IsEmailVerified = true;
            _userRepository.UpdateAccountInfo(user);
            return true;
        }
    }
}