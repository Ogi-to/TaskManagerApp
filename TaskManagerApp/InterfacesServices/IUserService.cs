using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IUserService
    {
        public void RegisterUser(RegisterUserDto registerUserDto);
        public void LogInUser(LoginUserDto loginUserDto);
        public UserDto GetUserById(int id);

    }
}
