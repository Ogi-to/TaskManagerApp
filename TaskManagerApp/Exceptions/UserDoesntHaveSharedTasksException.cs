namespace TaskManagerApp.Exceptions
{
    public class UserDoesntHaveSharedTasksException : Exception
    {
        public UserDoesntHaveSharedTasksException() : base("This user does not have any shared tasks")
        {
            
        }
    }
}
