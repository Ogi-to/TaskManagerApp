using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTOS
{
    public class AddCalendarMemberDto
    {
        [Required]
        public int CalendarId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}