using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class UserStats
    {
        [Key]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        public int HighestStreak { get; set; } = 0;

        public int TasksCompleted { get; set; } = 0;

        public int ChallengesCompleted { get; set; } = 0;
        public int TotalPoints { get; set; } = 0;

    }
}
