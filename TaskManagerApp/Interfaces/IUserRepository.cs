using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public User Get();
        public User Get(User item);

        public void CreateAccount();
        public List<User> GetAllUsers();

        public void CreateAccount(User item);

        public void UpdateAccount(User item);

        public void SendRequest();

        public void RespondToRequest();

        public List<User> GetFriendsList();

        public User GetFriendByName(User item);
    }
}
