using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

<<<<<<< Updated upstream
        public enum TaskState
=======
        public enum StateType
>>>>>>> Stashed changes
        {
            NotStarted,
            InProgress,
            Completed,
            Overdue
<<<<<<< Updated upstream
        }
=======
        };


>>>>>>> Stashed changes
    }
}
