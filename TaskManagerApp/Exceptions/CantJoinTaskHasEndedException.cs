namespace TaskManagerApp.Exceptions
{
    public class CantJoinTaskHasEndedException : Exception
    {
        public CantJoinTaskHasEndedException() : base("You can not join this task because it has ended!")
        {
            
        }
    }
}
