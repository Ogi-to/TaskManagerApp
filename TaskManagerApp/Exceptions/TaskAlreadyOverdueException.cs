namespace TaskManagerApp.Exceptions
{
    public class TaskAlreadyOverdueException : Exception
    {
        public TaskAlreadyOverdueException() : base("The task is already overdue.")
        { }
    }
}
