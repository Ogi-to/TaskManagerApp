using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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



        Task IUserService.LogInUser(LoginUserDto loginUserDto)
        {
            throw new NotImplementedException();
        }

        Task IUserService.RegisterUser(RegisterUserDto registerUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
