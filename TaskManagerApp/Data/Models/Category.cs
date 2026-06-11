using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public List<TasksCategories> TasksCategories { get; set; } = new List<TasksCategories>();
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
    }
}
