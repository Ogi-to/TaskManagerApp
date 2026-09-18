using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IUserService
    {
        Task RegisterUser(RegisterUserDto registerUserDto);

        Task<UserDto> LogInUser(LoginUserDto loginUserDto);

        Task<UserDto> GetUserById(int id);

        Task DeleteAccount(int userId);

        Task VerifyEmail(VerifyEmailDto verifyEmailDto);

        Task SendVerificationCode(string email);

        Task<UserDto?> GetUserByEmail(string email);

        Task<bool> IsEmailVerified(string email);

        Task UpdateStreak(User user);

        Task UpdateRank(User user);

        Task UpdatePoints(int userId, int points);
    }
}
