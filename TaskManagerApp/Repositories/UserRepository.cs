using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;
using static TaskManagerApp.Data.Models.State;

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
            _context.Users.Add(item);
            _context.SaveChanges();
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
            return _context.Users.Where(u => u.Username == item.Username).Include(u => u.Rank).Include(u => u.Challenges).Include(u => u.Stats).FirstOrDefault();
        }


        public void RespondToRequest(UsersRelations relation)
        {
            UsersRelations relationToModify = _context.UsersRelations.Where(i => i.InitiatorId == relation.InitiatorId).Where(r => r.RelatedUserId == relation.RelatedUserId).FirstOrDefault();
            relationToModify.RelationStatus = relation.RelationStatus;
            relationToModify.RelationType = relation.RelationType;
            _context.SaveChanges();
        }

        public void SendRequest(UsersRelations relation)
        { 
            _context.UsersRelations.Add(relation);
            _context.SaveChanges();
        }

        public void UpdateAccountInfo(User item)
        {
            User userToModify = _context.Users.Where(u => u.Id == item.Id).FirstOrDefault();
            userToModify.Username = item.Username;
            userToModify.Email = item.Email;
            userToModify.PasswordHash = item.PasswordHash;
            _context.SaveChanges();
        }
        public void UpdateUserInfo(User item)
        {
            User userToModify = _context.Users.Where(u => u.Id == item.Id).FirstOrDefault();
            userToModify.Points = item.Points;
            userToModify.RankId = item.RankId;
            userToModify.Rank = item.Rank;
            userToModify.Streak = item.Streak;
            _context.SaveChanges();
        }
        public List<User> GetFriendsList(User item)
        {
            List<UsersRelations> initiatedRelations = _context.UsersRelations.Where(ur => ur.InitiatorId == item.Id && ur.RelationType == RelationType.Friend).Include(ur => ur.RelatedUser).ToList();
            List<UsersRelations> acceptedRelations = _context.UsersRelations.Where(ur => ur.RelatedUserId == item.Id && ur.RelationType == RelationType.Friend).Include(ur => ur.Initiator).ToList();

            List<User> initiatedFriendships = initiatedRelations.Select(ur => ur.RelatedUser).ToList();
            List<User> acceptedFriendships = acceptedRelations.Select(ur => ur.Initiator).ToList();

            return initiatedFriendships.Concat(acceptedFriendships).OrderBy(u => u.Username).ToList();
        }
    }
}
