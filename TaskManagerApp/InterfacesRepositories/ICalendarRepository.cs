using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ICalendarRepository
    {
        // ============================================
        // CALENDAR
        // ============================================

        Task<Calendar?> GetAsync(int calendarId);

        Task<List<Calendar>> GetUserCalendarsAsync(
            int userId);

        Task<Calendar?> GetUserCalendarAsync(
            int calendarId,
            int userId);

        Task AddAsync(
            Calendar calendar);

        Task DeleteAsync(
            int calendarId);


        // ============================================
        // MEMBERS
        // ============================================

        Task<bool> IsMemberAsync(
            int calendarId,
            int userId);

        Task AddMemberAsync(
            CalendarMember member);

        Task RemoveMemberAsync(
            int calendarId,
            int userId);
    }
}