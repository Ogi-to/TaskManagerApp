namespace TaskManagerApp.Data.Models
{
    public class TasksCategories
    {
        public int TaskId { get; set; }
        public int CategoryId { get; set; }

        public TaskItem Task { get; set; }
        public Category Category { get; set; }
    }
}
