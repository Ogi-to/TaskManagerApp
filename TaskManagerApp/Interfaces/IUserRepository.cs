using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserRepository
    {
        public User Get();

        public void CreateAccount();

        public void UpdateAccount();

        public void SendRequest();

        public void RespondToRequest();

        public void GetFriendsList();

        public void GetFriendByName();
    }
}
