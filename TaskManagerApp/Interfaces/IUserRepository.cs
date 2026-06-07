using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public User Get();
        public User Get(User item);

        public void CreateAccount();
        public void CreateAccount(User item);

        public void UpdateAccount();
        public void UpdateAccount(User item);

        public void SendRequest();

        public void RespondToRequest();

        public void GetFriendsList();

        public void GetFriendByName();
    }
}
