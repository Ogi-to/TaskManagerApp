using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class Challenge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Duration {  get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? Trophy { get; set; }


        [Required]
        public int Points { get; set; }

        [Required]
        public string Description { get; set; }


        public List<UsersChallenges> UsersChallenges { get; set; } = new List<UsersChallenges>();

        public int StateId { get; set; }
        public StateType State { get; set; }

    }
}
