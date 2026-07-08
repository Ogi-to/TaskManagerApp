namespace TaskManagerApp.Exceptions
{
    public class UserDoesntHaveTasksException : Exception
    {
        public UserDoesntHaveTasksException() : base("User doesn't have any tasks assigned.")
        {
        }
    }
}
