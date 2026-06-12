
﻿using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.Data.Models
{
    public class UsersTasks
    {

        public int TaskId { get; set; }

        public int UserId { get; set; }

        public TaskItem Task { get; set; }
        public User User { get; set; }
    }
}
