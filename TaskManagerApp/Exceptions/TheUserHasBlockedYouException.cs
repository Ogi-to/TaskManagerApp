namespace TaskManagerApp.Exceptions
{
    public class TheUserHasBlockedYouException: Exception
    {
        public TheUserHasBlockedYouException() : base("This user has blocked you! YOu can not send him a friend request!")
        {
        }
    }
}
