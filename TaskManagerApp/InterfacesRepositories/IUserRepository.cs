using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetAsync(int id);

        public Task<List<User>> GetAllUsersAsync();
        public Task<User> GetByEmailAsync(string email);
        public Task<User> GetByUsernameAsync(string username);
        public Task<User> GetByUserCodeAsync(string userCode);
        public Task<List<UsersRelations>> GetUnansweredRelationReceivedByUserIdAsync(int relatedUserId);

        public Task<User> CreateAccountAsync(User item);
        public Task DeleteAccountAsync(int id);

        public Task UpdateAccountInfoAsync(UpdateAccountDto item, int userId);

        public Task UpdateUserInfoAsync(UpdateUserDto item, int id);

        public Task UpdateUserReminders(ReminderSettingsDto reminderSettingsDto, int userId);

        public Task<List<User>> GetUsersWithStreaksAboutToEndAsync();
        public Task SendRequestAsync(UsersRelations relation);

        public Task RespondToRequestAsync(UsersRelations relation);

        public Task<List<User>> GetFriendsListAsync(User item);




    }
}