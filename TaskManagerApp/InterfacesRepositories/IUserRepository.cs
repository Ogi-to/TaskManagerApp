using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetAsync(int id);

        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetByEmailAsync(string email);
        public Task<User?> GetByUsernameAsync(string username);
        public Task<User?> GetByUserCodeAsync(string userCode);
        public Task<UsersRelations?> GetRelationAsync(int initiatorId, int relatedUserId);

        public Task<User> CreateAccountAsync(User item);
        public Task DeleteAccountAsync(int id);

        public Task UpdateAccountInfoAsync(User item);

        public Task UpdateUserInfoAsync(User item);

        public Task SendRequestAsync(UsersRelations relation);

        public Task<bool> RespondToRequestAsync(UsersRelations relation);

        public Task<List<User>> GetFriendsListAsync(User item);




    }
}