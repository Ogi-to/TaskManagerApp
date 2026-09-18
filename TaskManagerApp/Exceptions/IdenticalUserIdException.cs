namespace TaskManagerApp.Exceptions
{
    public class IdenticalUserIdException: Exception
    {
        public IdenticalUserIdException() : base("You can not invite yourself!")
        {
            
        }
    }
}
