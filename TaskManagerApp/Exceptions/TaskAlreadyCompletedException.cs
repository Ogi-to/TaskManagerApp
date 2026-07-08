namespace TaskManagerApp.Exceptions
{
    public class TaskAlreadyCompletedException : Exception
    {
        public TaskAlreadyCompletedException() : base("Task is already completed.") { }
    }
}
