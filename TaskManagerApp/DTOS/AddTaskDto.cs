using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagerApp.Data.Models;

namespace TaskManagerApp.DTOS
{
    public class AddTaskDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }


        public DateTime? EndDate { get; set; }
        // Foreign key
        public int UserId { get; set; }

        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
