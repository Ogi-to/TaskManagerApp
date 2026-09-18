namespace TaskManagerApp.Exceptions
{
    public class CantSendTaskHasEndedException : Exception
    {
        public CantSendTaskHasEndedException() : base("You can not invite friends for this task because it has ended!")
        {
            
        }
    }
}
