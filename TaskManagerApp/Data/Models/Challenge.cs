using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class Challenge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Duration {  get; set; }

        public int Level { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? Trophy { get; set; }

        public int Points { get; set; }

        [MinLength(10)]
        public string Description { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        public List<UsersChallenges> UsersChallenges { get; set; } = new List<UsersChallenges>();


        public int StateId { get; set; }
        public State State { get; set; }

    }
}
