using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class CalendarMember
    {
        [Key]
        public int Id { get; set; }

        public int CalendarId { get; set; }

        [ForeignKey(nameof(CalendarId))]
        public Calendar Calendar { get; set; } = null!;

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}