namespace TaskManagerApp.Exceptions
{
    public class TaskDoesntHaveSharedUsersException : Exception
    {
        public TaskDoesntHaveSharedUsersException() : base("This task does not have any shared users!")
        {
            
        }
    }
}
