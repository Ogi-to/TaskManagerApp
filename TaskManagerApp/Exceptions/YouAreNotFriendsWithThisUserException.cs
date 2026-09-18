namespace TaskManagerApp.Exceptions
{
    public class YouAreNotFriendsWithThisUserException : Exception
    {
        public YouAreNotFriendsWithThisUserException() : base("You are no friends with this user!")
        {
            
        }
    }
}
