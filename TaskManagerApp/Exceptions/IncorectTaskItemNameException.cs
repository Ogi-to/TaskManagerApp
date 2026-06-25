namespace TaskManagerApp.Exceptions
{
    public class IncorectTaskItemNameException : Exception
    {
        public IncorectTaskItemNameException(string taskName) : base($"The task name '{taskName}' is not in a valid format.")
        {
        }
    }
}
