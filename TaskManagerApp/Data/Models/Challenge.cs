using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class Challenge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

<<<<<<< Updated upstream
        public DateTime Duration {  get; set; }

        public int Level { get; set; }

=======
        [Required]
        public DateTime Duration {  get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
>>>>>>> Stashed changes
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? Trophy { get; set; }

<<<<<<< Updated upstream
        public int Points { get; set; }

        [MinLength(10)]
        public string Description { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        public List<UsersChallenges> UsersChallenges { get; set; } = new List<UsersChallenges>();
=======
        [Required]
        public int Points { get; set; }

        [Required]
        public string Description { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        public List<ChallengesUsers> ChallengesUsers { get; set; } = new List<ChallengesUsers>();

>>>>>>> Stashed changes


        public int StateId { get; set; }
        public State State { get; set; }

    }
}
