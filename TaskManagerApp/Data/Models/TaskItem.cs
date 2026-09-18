using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; } = null;

        [Required]
        public DateTime StartDate { get; set; }


        public DateTime? EndDate { get; set; }
        public StateType State { get; set; }

        public int Priority { get; set; } = 3;
        // Foreign key
        public int UserId { get; set; }

        public int? CalendarId { get; set; }

        public Calendar? Calendar { get; set; }
        // Navigation property
        public User User { get; set; }

        public List<TasksCategories> TasksCategories { get; set; } = new List<TasksCategories>();

    }
}
 
