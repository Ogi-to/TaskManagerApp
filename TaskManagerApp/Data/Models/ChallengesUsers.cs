using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.Data.Models
{
    public class ChallengesUsers
    {
        public int UserId { get; set; }

        public int ChallengeId { get; set; }

        public User User { get; set; }
        public Challenge Challenge { get; set; }

        [Required]
        public int StateId { get; set; }
        public State State { get; set; }

    }
}
