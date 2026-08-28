using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
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
            return await _context.Users.ToListAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id); // or throw exception

            var userTasks = await _context.TaskItems
            .Where(t => t.UserId == id)
            .ToListAsync();

            var taskIds = userTasks.Select(t => t.Id).ToList();

            var taskCategories = await _context.TasksCategories
                .Where(tc => taskIds.Contains(tc.TaskId))
                .ToListAsync();

            var relations = await _context.UsersRelations
                .Where(r => r.InitiatorId == id || r.RelatedUserId == id)
                .ToListAsync();

            var challenges = await _context.UsersChallenges
                .Where(uc => uc.UserId == id)
                .ToListAsync();

            var taskParticipants = await _context.TasksParticipants
                .Where(tp => tp.UserId == id)
                .ToListAsync();

            var stats = await _context.UserStats
                .FirstOrDefaultAsync(s => s.UserId == id);

            _context.TasksParticipants.RemoveRange(taskParticipants);
            _context.TasksCategories.RemoveRange(taskCategories);
            _context.TaskItems.RemoveRange(userTasks);
            _context.UsersRelations.RemoveRange(relations);
            _context.UsersChallenges.RemoveRange(challenges);
            

            if (stats != null)
                _context.UserStats.Remove(stats);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }


        public async Task<User?> GetAsync(int id)
        {
            return await _context.Users.Include(u => u.TaskItems).Include(u => u.UsersChallenges).ThenInclude(uc => uc.Challenge).FirstOrDefaultAsync(u => u.Id == id);
            
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Where(u => u.Email == email).Include(u => u.UsersChallenges)
                 .ThenInclude(uc => uc.Challenge).Include(u => u.TaskItems)
                 .Include(u => u.Stats).FirstOrDefaultAsync();
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.Where(u => u.Username == username).Include(u => u.UsersChallenges)
                 .ThenInclude(uc => uc.Challenge).Include(u => u.TaskItems)
                 .Include(u => u.Stats).FirstOrDefaultAsync();
        }
        public async Task<User?> GetByUserCodeAsync(string userCode)
        {
            return await _context.Users.Where(u => u.UserCode == userCode).Include(u => u.UsersChallenges)
                 .ThenInclude(uc => uc.Challenge).Include(u => u.TaskItems)
                 .Include(u => u.Stats).FirstOrDefaultAsync();
        }


        public async Task<List<UsersRelations>> GetUserInvitesTodayAsync(int userId)
        {
            var today = DateTime.UtcNow.Date;
            List<UsersRelations> usersRelations = await _context.UsersRelations.Where(ur => ur.InitiatorId == userId && ur.CreatedAt.Date == today).ToListAsync();
            return usersRelations;
        }

        public async Task<UsersRelations> GetUserRelationAsync(int user1Id, int user2Id)
        {
            //DOESNT MATTER WHO IS THE INITIATOR. ONLY MATTER IF THE USERS ARE IN A RELATION
            UsersRelations usersRelations = await _context.UsersRelations.Where(ur => (ur.InitiatorId == user1Id || ur.InitiatorId == user2Id) 
            && (ur.RelatedUserId == user1Id || ur.RelatedUserId == user2Id)).FirstOrDefaultAsync();
            return usersRelations;
        }

        public async Task DeleteAllUnansweredUserRelationsByMoreThanAMonth()
        {
            var today = DateTime.UtcNow.Date;
            List<UsersRelations> usersRelations = await _context.UsersRelations.Where(ur => (ur.CreatedAt.Date.AddDays(30) <= today) 
            && (ur.RelationStatus == RelationStatus.Pending)).ToListAsync();
            _context.RemoveRange(usersRelations);
            _context.SaveChanges();
        }

        

        public async Task<List<UsersRelations>> GetUnansweredRelationReceivedByUserIdAsync(int relatedUserId)
        {
            return await _context.UsersRelations
                .Include(r => r.Initiator)
                .Include(r => r.RelatedUser)
                .Where(r => r.RelatedUserId == relatedUserId &&
                            r.RelationStatus == RelationStatus.Pending)
                .ToListAsync();
        }

        //public void RespondToRequest(UsersRelations relation)
        //{
        //UsersRelations relationToModify = _context.UsersRelations.Where(i => i.InitiatorId == relation.InitiatorId).Where(r => r.RelatedUserId == relation.RelatedUserId).FirstOrDefault();
        //relationToModify.RelationStatus = relation.RelationStatus;
        //relationToModify.RelationType = relation.RelationType;
        //_context.SaveChanges();
        //}

        public async Task RespondToRequestAsync(UsersRelations relation)
        {
            var relationToModify = await _context.UsersRelations
                .FirstOrDefaultAsync(r =>
                    r.InitiatorId == relation.InitiatorId &&
                    r.RelatedUserId == relation.RelatedUserId);

            relationToModify.RelationStatus = relation.RelationStatus;
            relationToModify.RelationType = relation.RelationType;
            relation.TimeOfAction = relation.TimeOfAction;

            await _context.SaveChangesAsync();
        }

        //public void SendRequest(UsersRelations relation)
        //{ 
        //    _context.UsersRelations.Add(relation);
        //    _context.SaveChanges();
        //}

        public async Task SendRequestAsync(UsersRelations relation)
        {
            relation.RelationStatus = RelationStatus.Pending;
            relation.RelationType = null;
            relation.CreatedAt = DateTime.UtcNow;
            relation.TimeOfAction = null;
            await _context.UsersRelations.AddAsync(relation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountInfoAsync(UpdateAccountDto item, int userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            user.Username = item.Username;
            user.Email = item.Email;
            user.PasswordHash = item.Password;
            user.IsEmailVerified = item.IsEmailVerified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserReminders(ReminderSettingsDto reminderSettingsDto, int userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            user.ReminderStartBefore = reminderSettingsDto.ReminderStartBefore;
            user.ReminderInterval = reminderSettingsDto.ReminderInterval;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserInfoAsync(UpdateUserDto item, int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.Points = item.Points;
            user.RankId = item.RankId;
            user.Streak = item.Streak;
            user.LastActive = item.LastActive;

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

        public async Task<List<User>> GetUsersWithStreaksAboutToEndAsync()
        {
            var today = DateTime.UtcNow.Date;
            List<User> users = await _context.Users.Where(u => u.LastActive != default && u.LastActive < today).ToListAsync();
            return users;
        }


    }
}