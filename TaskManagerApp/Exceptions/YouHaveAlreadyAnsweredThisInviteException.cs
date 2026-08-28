namespace TaskManagerApp.Exceptions
{
    public class YouHaveAlreadyAnsweredThisInviteException : Exception
    {
        public YouHaveAlreadyAnsweredThisInviteException() : base("You have already answered this invite!")
        { }
    }
}
