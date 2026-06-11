<<<<<<< Updated upstream
﻿using System.ComponentModel.DataAnnotations;
=======
﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
=======
        [StringLength(100, MinimumLength = 3)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
>>>>>>> Stashed changes
        public string PasswordHash { get; set; }

        public int Streak { get; set; } = 0;

        public int Points { get; set; } = 0;

        public int RankId { get; set; }
        public Rank Rank { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserCode { get; set; }

<<<<<<< Updated upstream
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
=======
        public List<Challenge> Challenges { get; set; } = new List<Challenge>();
        public List<ChallengesUsers> UsersChallenges { get; set; } = new List<ChallengesUsers>();

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public List<UsersTasks> TasksUsers { get; set; } = new List<UsersTasks>();

        public List<UsersRelations> SentRelations { get; set; } = new List<UsersRelations>();

        public List<UsersRelations> ReceivedRelations { get; set; } = new List<UsersRelations>();

        public UserStats Stats { get; set; }
    }
}
>>>>>>> Stashed changes
