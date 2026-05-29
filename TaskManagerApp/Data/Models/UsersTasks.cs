namespace TaskManagerApp.Data.Models
{
    public class UsersTasks
    {
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public User User { get; set; }
        public TaskItem Task { get; set; }
    }
}
