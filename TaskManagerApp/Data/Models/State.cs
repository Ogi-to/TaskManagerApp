using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public StateType Type { get; set; }
    }
    public enum StateType
    {
        NotStarted,
        InProgress,
        Completed,
        Overdue
    }
}
