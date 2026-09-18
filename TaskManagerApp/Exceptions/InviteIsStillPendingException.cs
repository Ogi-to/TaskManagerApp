namespace TaskManagerApp.Exceptions
{
    public class InviteIsStillPendingException : Exception
    {
        public InviteIsStillPendingException() : base("Your invite is still pending. You can not send another one!")
        {
            
        }
    }
}
