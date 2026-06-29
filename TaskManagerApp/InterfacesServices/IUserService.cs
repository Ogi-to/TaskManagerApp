using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IUserService
    {
        public Task RegisterUser(RegisterUserDto registerUserDto);
        public Task LogInUser(LoginUserDto loginUserDto);
        public Task<UserDto> GetUserById(int id);

    }
}
