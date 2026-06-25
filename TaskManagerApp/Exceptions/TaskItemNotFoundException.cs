namespace TaskManagerApp.Exceptions
{
    public class TaskItemNotFoundException : Exception
    {
        public TaskItemNotFoundException() : base("Task not found.")
        { }
    }
}
