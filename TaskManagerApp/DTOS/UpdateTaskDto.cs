using System.ComponentModel.DataAnnotations;
using TaskManagerApp.Data.Models;

namespace TaskManagerApp.DTOS
{
    public class UpdateTaskDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }


        public DateTime? EndDate { get; set; }

        public StateType State { get; set; }
        // Foreign key
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
