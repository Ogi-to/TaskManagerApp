using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public User Get(User item);

        public List<User> GetAllUsers();

        public void CreateAccount(User item);

        public void UpdateAccountInfo(User item);

        public void UpdateUserInfo(User item);

        public void SendRequest(UsersRelations relation);

        public void RespondToRequest(UsersRelations relation);

        public List<User> GetFriendsList(User item);

        public User GetFriendByName(User item);
    }
}
