using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetAsync(int id);

        public Task<List<User>> GetAllUsers();
        public Task<User?> GetByEmail(string email);
        public Task<User?> GetByUsername(string username);
        public Task<User?> GetByUserCode(string userCode);
        public Task<UsersRelations> GetRelation(int initiatorId, int relatedUserId);

        public Task<User> CreateAccount(User item);

        public void UpdateAccountInfo(User item);

        public void UpdateUserInfo(User item);

        public void SendRequest(UsersRelations relation);

        public bool RespondToRequest(UsersRelations relation);

        public List<User> GetFriendsList(User item);

        
    }
}
