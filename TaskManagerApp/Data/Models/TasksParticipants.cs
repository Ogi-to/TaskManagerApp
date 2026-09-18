
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaskManagerApp.Data.Models
{
    public class TasksParticipants
    {
        public int TaskId { get; set; }
        public TaskItem TaskItem { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public Status Status{ get; set; }

        

    }
    public enum Status
    {
        Pending,
        Accepted,
        Declined
    }

}
