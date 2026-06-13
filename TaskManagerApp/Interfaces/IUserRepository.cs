using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public User Get(User item);

        public List<User> GetAllUsers();
        public User GetByEmail(string email);
        public User GetByUsername(string username);
        public User GetByUserCode(string userCode);
        public UsersRelations GetRelation(int initiatorId, int relatedUserId);

        public User CreateAccount(User item);

        public void UpdateAccountInfo(User item);

        public void UpdateUserInfo(User item);

        public void SendRequest(UsersRelations relation);

        public bool RespondToRequest(UsersRelations relation);

        public List<User> GetFriendsList(User item);

        
    }
}
