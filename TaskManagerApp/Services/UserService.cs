using TaskManagerApp.DTOS;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public UserDto GetUserById(int id)
        {
            var user = _userRepository.Get(id);

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

        public void LogInUser(LoginUserDto loginUserDto)
        {
            throw new NotImplementedException();
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
