using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public int Streak { get; set; } = 0;

        public int Points { get; set; } = 0;

        public int RankId { get; set; }
        public Rank Rank { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserCode { get; set; }

        // Challenges
        public List<Challenge> Challenges { get; set; }
             = new();
        public List<UsersChallenges> UsersChallenges { get; set; }
            = new();

        // Tasks
        public List<TaskItem> TaskItems { get; set; }
            = new();
        public List<UsersTasks> UsersTasks { get; set; }
            = new();

        // Relationships
        public List<UsersRelations> SentRelations { get; set; }
            = new();

        public List<UsersRelations> ReceivedRelations { get; set; }
            = new();

        public UserStats Stats { get; set; }
    }
}