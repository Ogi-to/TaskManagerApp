using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly TaskManagerDbContext _context;

        public CalendarRepository(TaskManagerDbContext context)
        {
            _context = context;
        }


        // ============================================
        // GET CALENDAR
        // ============================================

        public async Task<Calendar?> GetAsync(int calendarId)
        {
            return await _context.Calendars
                .Include(c => c.Owner)
                .Include(c => c.Members)
                    .ThenInclude(m => m.User)
                .Include(c => c.Tasks)
                    .ThenInclude(t => t.User)
                .Include(c => c.Tasks)
                    .ThenInclude(t => t.TasksCategories)
                        .ThenInclude(tc => tc.Category)
                .FirstOrDefaultAsync(c => c.Id == calendarId);
        }


        // ============================================
        // GET ALL USER CALENDARS
        // ============================================

        public async Task<List<Calendar>> GetUserCalendarsAsync(
        int userId)
        {
            return await _context.Calendars

                // Собственик на календара
                .Include(c => c.Owner)

                // Членове на календара + информация за потребителя
                .Include(c => c.Members)
                    .ThenInclude(m => m.User)

                // Задачи + потребителят, който ги е създал
                .Include(c => c.Tasks)
                    .ThenInclude(t => t.User)

                // Задачи + категории
                .Include(c => c.Tasks)
                    .ThenInclude(t => t.TasksCategories)
                        .ThenInclude(tc => tc.Category)

                // Календарът трябва да е:
                // 1. собствен на потребителя
                // ИЛИ
                // 2. потребителят да е негов член
                .Where(c =>
                    c.OwnerId == userId ||
                    c.Members.Any(m => m.UserId == userId))

                .OrderBy(c => c.Name)

                .ToListAsync();
        }


        // ============================================
        // GET USER CALENDAR
        // ============================================

        public async Task<Calendar?> GetUserCalendarAsync(
            int calendarId,
            int userId)
        {
            return await _context.Calendars

                .Include(c => c.Owner)

                .Include(c => c.Members)
                    .ThenInclude(m => m.User)

                .Include(c => c.Tasks)
                    .ThenInclude(t => t.User)

                .Include(c => c.Tasks)
                    .ThenInclude(t => t.TasksCategories)
                        .ThenInclude(tc => tc.Category)

                .FirstOrDefaultAsync(c =>
                    c.Id == calendarId &&
                    (
                        c.OwnerId == userId ||
                        c.Members.Any(m => m.UserId == userId)
                    ));
        }


        // ============================================
        // CHECK MEMBER
        // ============================================

        public async Task<bool> IsMemberAsync(
            int calendarId,
            int userId)
        {
            return await _context.CalendarMembers
                .AnyAsync(m =>
                    m.CalendarId == calendarId &&
                    m.UserId == userId);
        }


        // ============================================
        // ADD CALENDAR
        // ============================================

        public async Task AddAsync(Calendar calendar)
        {
            await _context.Calendars.AddAsync(calendar);

            await _context.SaveChangesAsync();
        }


        // ============================================
        // ADD MEMBER
        // ============================================

        public async Task AddMemberAsync(
            CalendarMember member)
        {
            await _context.CalendarMembers.AddAsync(member);

            await _context.SaveChangesAsync();
        }


        // ============================================
        // REMOVE MEMBER
        // ============================================

        public async Task RemoveMemberAsync(
            int calendarId,
            int userId)
        {
            var member =
                await _context.CalendarMembers
                    .FirstOrDefaultAsync(m =>
                        m.CalendarId == calendarId &&
                        m.UserId == userId);

            if (member == null)
            {
                return;
            }

            _context.CalendarMembers.Remove(member);

            await _context.SaveChangesAsync();
        }


        // ============================================
        // DELETE CALENDAR
        // ============================================

        public async Task DeleteAsync(
            int calendarId)
        {
            var calendar =
                await _context.Calendars
                    .FirstOrDefaultAsync(c =>
                        c.Id == calendarId);

            if (calendar == null)
            {
                return;
            }

            _context.Calendars.Remove(calendar);

            await _context.SaveChangesAsync();
        }
    }
}