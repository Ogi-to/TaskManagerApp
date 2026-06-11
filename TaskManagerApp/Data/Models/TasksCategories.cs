<<<<<<< Updated upstream
﻿namespace TaskManagerApp.Data.Models
{
    public class TasksCategories
    {
        public int TaskId { get; set; }
=======
﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.Data.Models
{
    public class TasksCategories
    {

        public int TaskId { get; set; }

>>>>>>> Stashed changes
        public int CategoryId { get; set; }

        public TaskItem Task { get; set; }
        public Category Category { get; set; }
    }
}
