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
<<<<<<< Updated upstream
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
=======
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

>>>>>>> Stashed changes
        public DateTime? EndDate { get; set; }

        public int StateId { get; set; }
        public State State { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<TasksCategories> TasksCategories { get; set; } = new List<TasksCategories>();

        public List<User> Users { get; set; } = new List<User>();
<<<<<<< Updated upstream
        public List<UsersTasks> UsersTasks { get; set; } = new List<UsersTasks>();
    }
}

=======
        public List<UsersTasks> TasksUsers { get; set; } = new List<UsersTasks>();
    }
}
>>>>>>> Stashed changes
