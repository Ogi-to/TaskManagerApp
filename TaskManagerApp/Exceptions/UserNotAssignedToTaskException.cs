namespace TaskManagerApp.Exceptions
{
    public class UserNotAssignedToTaskException : Exception
    {
        public UserNotAssignedToTaskException() : base("User is not assigned to this task.")
        {
        }
    
    }
}
