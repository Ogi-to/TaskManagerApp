namespace TaskManagerApp.Exceptions
{
    public class UserHasntJoinedAnyChallengesException : Exception
    {
        public UserHasntJoinedAnyChallengesException() : base("User hasn't joined any challenges.")
        {
            
        }
    }
}
