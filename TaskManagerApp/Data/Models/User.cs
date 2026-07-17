
﻿using Microsoft.EntityFrameworkCore;
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
        [StringLength(100, MinimumLength = 3)]
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; } = false;

        [Required]
        [MinLength(8)]
        public string PasswordHash { get; set; } = string.Empty;

        public int Streak { get; set; } = 0;

        public int Points { get; set; } = 0;

        public int RankId { get; set; }
        public Rank Rank { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastActive {  get; set; }
        [Required]
        [StringLength(8)]
        public string UserCode { get; set; }

        public int ReminderStartBefore { get; set; } = 60; // in minutes, default is 60 minutes
        public int ReminderInterval { get; set; } = 20; // in minutes, default is 20 minutes

        public List<UsersChallenges> UsersChallenges { get; set; } = new List<UsersChallenges>();

        public List<TaskItem> TaskItems { get; set; }

        public List<UsersRelations> SentRelations { get; set; } = new List<UsersRelations>();

        public List<UsersRelations> ReceivedRelations { get; set; } = new List<UsersRelations>();

        public UserStats Stats { get; set; }
    }
}

