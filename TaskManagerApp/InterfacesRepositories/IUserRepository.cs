using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetAsync(int id);

        public List<User> GetAllUsers();
        public User GetByEmail(string email);
        public User GetByUsername(string username);
        public User GetByUserCode(string userCode);
        public UsersRelations GetRelation(int initiatorId, int relatedUserId);

        public User CreateAccount(User item);
        public void DeleteAccount(int id);

        public void UpdateAccountInfo(User item);

        public Task UpdateUserInfo(User item);

        public void SendRequest(UsersRelations relation);

        public bool RespondToRequest(UsersRelations relation);

        public List<User> GetFriendsList(User item);




    }
}