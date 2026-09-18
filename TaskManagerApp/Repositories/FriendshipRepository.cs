using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly TaskManagerDbContext _context;

        public FriendshipRepository(TaskManagerDbContext context)
        {
            _context = context;
        }


        // ============================================
        // ADD FRIENDSHIP
        // ============================================

        public async Task<Friendship> AddAsync(
            Friendship friendship)
        {
            await _context.Friendships.AddAsync(friendship);

            await _context.SaveChangesAsync();

            return friendship;
        }


        // ============================================
        // GET BY ID
        // ============================================

        public async Task<Friendship?> GetAsync(
            int friendshipId)
        {
            return await _context.Friendships

                .Include(f => f.Sender)

                .Include(f => f.Receiver)

                .FirstOrDefaultAsync(
                    f => f.Id == friendshipId);
        }


        // ============================================
        // GET BETWEEN USERS
        // ============================================

        public async Task<Friendship?>
            GetBetweenUsersAsync(
                int userId1,
                int userId2)
        {
            return await _context.Friendships

                .Include(f => f.Sender)

                .Include(f => f.Receiver)

                .FirstOrDefaultAsync(f =>
                    (f.SenderId == userId1 &&
                     f.ReceiverId == userId2)

                    ||

                    (f.SenderId == userId2 &&
                     f.ReceiverId == userId1));
        }


        // ============================================
        // GET USER FRIENDSHIPS
        // ============================================

        public async Task<List<Friendship>>
            GetUserFriendshipsAsync(
                int userId)
        {
            return await _context.Friendships

                .Include(f => f.Sender)

                .Include(f => f.Receiver)

                .Where(f =>
                    f.SenderId == userId ||
                    f.ReceiverId == userId)

                .OrderByDescending(
                    f => f.CreatedAt)

                .ToListAsync();
        }


        // ============================================
        // GET PENDING REQUESTS
        // ============================================

        public async Task<List<Friendship>>
            GetPendingRequestsAsync(
                int userId)
        {
            return await _context.Friendships

                .Include(f => f.Sender)

                .Include(f => f.Receiver)

                .Where(f =>
                    f.ReceiverId == userId &&
                    f.Status == FriendshipStatus.Pending)

                .OrderByDescending(
                    f => f.CreatedAt)

                .ToListAsync();
        }


        // ============================================
        // UPDATE
        // ============================================

        public async Task UpdateAsync(
            Friendship friendship)
        {
            _context.Friendships.Update(friendship);

            await _context.SaveChangesAsync();
        }


        // ============================================
        // DELETE
        // ============================================

        public async Task DeleteAsync(
            int friendshipId)
        {
            var friendship =
                await _context.Friendships
                    .FirstOrDefaultAsync(
                        f => f.Id == friendshipId);

            if (friendship == null)
            {
                return;
            }

            _context.Friendships.Remove(friendship);

            await _context.SaveChangesAsync();
        }
    }
}