using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private TaskManagerDbContext _context;
        public UserRepository(TaskManagerDbContext context)
        {
            _context = context;
        }


        public void CreateAccount(User item)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.OrderBy(u => u.Id).ToList();
        }

        public User Get(User item)
        {
            return _context.Users.Where(u => u.Id == item.Id).Include(u => u.TaskItems).Include(u => u.Challenges).FirstOrDefault();
        }

        public User GetFriendByName(User item)
        {
            throw new NotImplementedException();
        }


        public void RespondToRequest()
        {
            throw new NotImplementedException();
        }

        public void SendRequest()
        {
            throw new NotImplementedException();
        }

        public void UpdateAccount(User item)
        {
            throw new NotImplementedException();
        }

        public List<User> GetFriendsList()
        {
            throw new NotImplementedException();
        }
    }
}
