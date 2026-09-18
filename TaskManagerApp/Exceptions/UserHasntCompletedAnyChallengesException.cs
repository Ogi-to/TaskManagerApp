namespace TaskManagerApp.Exceptions
{
    public class UserHasntCompletedAnyChallengesException : Exception
    {
        public UserHasntCompletedAnyChallengesException() : base("User hasn't completed any challenges.")
        {
            
        }
    }
}
