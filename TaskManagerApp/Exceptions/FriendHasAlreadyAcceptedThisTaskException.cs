namespace TaskManagerApp.Exceptions
{
    public class FriendHasAlreadyAcceptedThisTaskException : Exception
    {
        public FriendHasAlreadyAcceptedThisTaskException() : base("Your friend and you already share this task!") 
        {
            
        }
    }
}
