using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface ICalendarService
    {
        Task<CalendarDto> CreateCalendarAsync(
            int userId,
            CreateCalendarDto dto);

        Task<List<CalendarDto>> GetUserCalendarsAsync(
            int userId);

        Task<CalendarDto> GetCalendarAsync(
            int calendarId,
            int userId);

        Task AddMemberAsync(
            int userId,
            AddCalendarMemberDto dto);

        Task RemoveMemberAsync(
            int userId,
            int calendarId,
            int memberId);

        Task DeleteCalendarAsync(
            int userId,
            int calendarId);
    }
}