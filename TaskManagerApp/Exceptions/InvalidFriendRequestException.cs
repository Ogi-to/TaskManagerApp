namespace TaskManagerApp.Exceptions
{
    public class InvalidFriendRequestException : Exception
    {
        public InvalidFriendRequestException() : base("You can't send a friend request to yourself") { }
    }
}
