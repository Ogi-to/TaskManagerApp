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


        public async Task<User> CreateAccountAsync(User item)
        {
            item.RankId = 1;
            _context.Users.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.OrderBy(u => u.Id).ToListAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var userToDelete = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            var userTasks = await _context.TaskItems.Where(t => t.UserId == id).ToListAsync();
            var userRelations = await _context.UsersRelations.Where(r => r.InitiatorId == id).ToListAsync();
            var userChalllenges = await _context.UsersChallenges.Where(uc => uc.UserId == id).ToListAsync();
            var userStats = await _context.UserStats.Where(s => s.UserId == id).FirstOrDefaultAsync();
            _context.TaskItems.RemoveRange(userTasks);
            _context.UsersRelations.RemoveRange(userRelations);
            _context.UsersChallenges.RemoveRange(userChalllenges);
            _context.UserStats.Remove(userStats);

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetAsync(int id)
        {
            return await _context.Users
             .Include(u => u.Rank)
             .Include(u => u.TaskItems)
             .Include(u => u.UsersChallenges)
                 .ThenInclude(uc => uc.Challenge)
             .Include(u => u.SentRelations)
             .Include(u => u.ReceivedRelations)
             .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Where(u => u.Email == email).Include(u => u.Rank).Include(u => u.UsersChallenges)
                 .ThenInclude(uc => uc.Challenge).Include(u => u.TaskItems)
                 .Include(u => u.Stats).FirstOrDefaultAsync();
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.Where(u => u.Username == username).Include(u => u.Rank).Include(u => u.UsersChallenges)
                 .ThenInclude(uc => uc.Challenge).Include(u => u.TaskItems)
                 .Include(u => u.Stats).FirstOrDefaultAsync();
        }
        public async Task<User?> GetByUserCodeAsync(string userCode)
        {
            return await _context.Users.Where(u => u.UserCode == userCode).Include(u => u.Rank).Include(u => u.UsersChallenges)
                 .ThenInclude(uc => uc.Challenge).Include(u => u.TaskItems)
                 .Include(u => u.Stats).FirstOrDefaultAsync();
        }

        public async Task<UsersRelations?> GetRelationAsync(int initiatorId, int relatedUserId)
        {
            return await _context.UsersRelations.FirstOrDefaultAsync(r => r.InitiatorId == initiatorId && r.RelatedUserId == relatedUserId);
        }


        //public void RespondToRequest(UsersRelations relation)
        //{
        //UsersRelations relationToModify = _context.UsersRelations.Where(i => i.InitiatorId == relation.InitiatorId).Where(r => r.RelatedUserId == relation.RelatedUserId).FirstOrDefault();
        //relationToModify.RelationStatus = relation.RelationStatus;
        //relationToModify.RelationType = relation.RelationType;
        //_context.SaveChanges();
        //}

        public async Task<bool> RespondToRequestAsync(UsersRelations relation)
        {
            var relationToModify = await _context.UsersRelations
                .FirstOrDefaultAsync(r =>
                    r.InitiatorId == relation.InitiatorId &&
                    r.RelatedUserId == relation.RelatedUserId);

            if (relationToModify == null)
                return false;

            relationToModify.RelationStatus = relation.RelationStatus;
            relationToModify.RelationType = relation.RelationType;

            await _context.SaveChangesAsync();

            return true;
        }

        //public void SendRequest(UsersRelations relation)
        //{ 
        //    _context.UsersRelations.Add(relation);
        //    _context.SaveChanges();
        //}

        public async Task SendRequestAsync(UsersRelations relation)
        {
            await _context.UsersRelations.AddAsync(relation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountInfoAsync(User item)
        {
            User userToModify = _context.Users.Where(u => u.Id == item.Id).FirstOrDefault();
            userToModify.Username = item.Username;
            userToModify.Email = item.Email;
            userToModify.PasswordHash = item.PasswordHash;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserInfoAsync(User item)
        {
            User userToModify = _context.Users.Where(u => u.Id == item.Id).FirstOrDefault();
            userToModify.Points = item.Points;
            userToModify.RankId = item.RankId;
            userToModify.Rank = item.Rank;
            userToModify.Streak = item.Streak;
            userToModify.LastActive = item.LastActive;
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetFriendsListAsync(User item)
        {
            var initiatedRelations = await _context.UsersRelations
                .Where(ur => ur.InitiatorId == item.Id && ur.RelationType == RelationType.Friend)
                .Include(ur => ur.RelatedUser)
                .ToListAsync();

            var acceptedRelations = await _context.UsersRelations
                .Where(ur => ur.RelatedUserId == item.Id && ur.RelationType == RelationType.Friend)
                .Include(ur => ur.Initiator)
                .ToListAsync();

            var initiatedFriendships = initiatedRelations.Select(ur => ur.RelatedUser).ToList();
            var acceptedFriendships = acceptedRelations.Select(ur => ur.Initiator).ToList();

            return initiatedFriendships.Concat(acceptedFriendships).OrderBy(u => u.Username).ToList();
        }
    }
}