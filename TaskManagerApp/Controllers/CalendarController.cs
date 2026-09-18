using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.DTOS;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(
            ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }


        // ============================================
        // CREATE CALENDAR
        // ============================================

        [HttpPost]
        public async Task<IActionResult> CreateCalendar(
            [FromQuery] int userId,
            [FromBody] CreateCalendarDto dto)
        {
            try
            {
                var calendar =
                    await _calendarService.CreateCalendarAsync(
                        userId,
                        dto);

                return Created(
                    $"api/Calendar/{calendar.Id}",
                    calendar);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        error = ex.Message
                    });
            }
        }


        // ============================================
        // GET USER CALENDARS
        // ============================================

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserCalendars(
            int userId)
        {
            try
            {
                var calendars =
                    await _calendarService
                        .GetUserCalendarsAsync(userId);

                return Ok(calendars);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        error = ex.Message
                    });
            }
        }


        // ============================================
        // GET SINGLE CALENDAR
        // ============================================

        [HttpGet("{calendarId}")]
        public async Task<IActionResult> GetCalendar(
            int calendarId,
            [FromQuery] int userId)
        {
            try
            {
                var calendar =
                    await _calendarService
                        .GetCalendarAsync(
                            calendarId,
                            userId);

                return Ok(calendar);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        error = ex.Message
                    });
            }
        }


        // ============================================
        // ADD MEMBER
        // ============================================

        [HttpPost("member")]
        public async Task<IActionResult> AddMember(
            [FromQuery] int userId,
            [FromBody] AddCalendarMemberDto dto)
        {
            try
            {
                await _calendarService.AddMemberAsync(
                    userId,
                    dto);

                return Ok(
                    new
                    {
                        message =
                            "Member added successfully."
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        error = ex.Message
                    });
            }
        }


        // ============================================
        // REMOVE MEMBER
        // ============================================

        [HttpDelete(
            "{calendarId}/member/{memberId}")]
        public async Task<IActionResult> RemoveMember(
            int calendarId,
            int memberId,
            [FromQuery] int userId)
        {
            try
            {
                await _calendarService.RemoveMemberAsync(
                    userId,
                    calendarId,
                    memberId);

                return Ok(
                    new
                    {
                        message =
                            "Member removed successfully."
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        error = ex.Message
                    });
            }
        }


        // ============================================
        // DELETE CALENDAR
        // ============================================

        [HttpDelete("{calendarId}")]
        public async Task<IActionResult> DeleteCalendar(
            int calendarId,
            [FromQuery] int userId)
        {
            try
            {
                await _calendarService.DeleteCalendarAsync(
                    userId,
                    calendarId);

                return Ok(
                    new
                    {
                        message =
                            "Calendar deleted successfully."
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        error = ex.Message
                    });
            }
        }
    }
}