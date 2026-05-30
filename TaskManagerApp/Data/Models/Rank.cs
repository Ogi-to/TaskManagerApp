using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class Rank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
<<<<<<< Updated upstream
        [StringLength(50)]
=======
        [StringLength(50, MinimumLength = 3)]
>>>>>>> Stashed changes
        public string Name { get; set; }

        [Required]
        public int StartPoints { get; set; }

        [Required]
        public int EndPoints { get; set; }
    }
}
