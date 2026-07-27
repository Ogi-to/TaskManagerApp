namespace TaskManagerApp.Exceptions
{
    public class YouAreAlreadyFriendsException : Exception
    {
        public YouAreAlreadyFriendsException() : base("You are already friends with this user!")
        {
            
        }
    }
}
