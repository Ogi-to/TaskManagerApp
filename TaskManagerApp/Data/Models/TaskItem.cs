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
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int StateId { get; set; }
        public State State { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<TasksCategories> TasksCategories { get; set; } = new List<TasksCategories>();

        public List<User> Users { get; set; } = new List<User>();
        public List<UsersTasks> UsersTasks { get; set; } = new List<UsersTasks>();
    }
}

