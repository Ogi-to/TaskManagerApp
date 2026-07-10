using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IUserService
    {
        public Task RegisterUser(RegisterUserDto registerUserDto);
        public Task<UserDto> LogInUser(LoginUserDto loginUserDto);
        public Task<UserDto> GetUserById(int id);
        public Task<bool> VerifyEmail(string email, string code);
        public Task UpdateStreak(User item);
        public Task UpdateRank(User item);
        public Task UpdatePoints(User item);
    }
}
