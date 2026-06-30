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
        private readonly IEmailCodeRepository _emailCodeRepository;
        private readonly IEmailCodeService _emailCodeService;
        private readonly HashPasswordService _hashPasswordService;

        public UserService(IUserRepository userRepository, IEmailCodeRepository emailCodeRepository , IEmailCodeService emailCodeService, HashPasswordService hashPasswordService)
        {
            _userRepository = userRepository;
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
            var testUser = await _userRepository.GetByUsername(registerUserDto.Username);
            if (testUser != null)
            {
                throw new UserNameAlreadyExistsException();
            }
            testUser = await _userRepository.GetByEmail(registerUserDto.Email);
            if (testUser != null)
            {
                throw new UserEmailAlreadyExistsException();
            }

            var user = new User
            {
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                PasswordHash = _hashPasswordService.HashPassword(registerUserDto.Password),
                IsEmailVerified = false

            };

            await _userRepository.CreateAccount(user);

            await _emailCodeService.SendVerificationCode(user.Email);
        }

        public async Task<UserDto> LogInUser(LoginUserDto loginUserDto)
        {
            var testUser = await _userRepository.GetByEmail(loginUserDto.Email);
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

        

        public async Task<bool> VerifyEmail(string email, string code)
        {
            var isVerified = await _emailCodeService.VerifyEmail(email, code);
            if (!isVerified)
            {
                throw new InvalidVerificationCodeException();
            }
            var user = await _userRepository.GetByEmail(email);
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
