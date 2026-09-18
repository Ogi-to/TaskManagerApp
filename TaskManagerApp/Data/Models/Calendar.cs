using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class Calendar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Color { get; set; } = "blue";

        // Потребителите, с които е споделен календарът
        public List<CalendarMember> Members { get; set; } = new();

        // Задачите в календара
        public List<TaskItem> Tasks { get; set; } = new();
    }
}