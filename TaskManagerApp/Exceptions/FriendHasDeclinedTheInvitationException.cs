namespace TaskManagerApp.Exceptions
{
    public class FriendHasDeclinedTheInvitationException : Exception
    {
        public FriendHasDeclinedTheInvitationException() : base("Yur friend has already declined your invitation!")
        {
            
        }
    }
}
