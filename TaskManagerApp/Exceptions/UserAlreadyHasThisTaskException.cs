namespace TaskManagerApp.Exceptions
{
    public class UserAlreadyHasThisTaskException : Exception
    {
        public UserAlreadyHasThisTaskException() : base("User already has this task assigned.")
        {
        }
    }
}
