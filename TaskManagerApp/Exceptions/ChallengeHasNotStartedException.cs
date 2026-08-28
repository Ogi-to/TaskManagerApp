namespace TaskManagerApp.Exceptions
{
    public class ChallengeHasNotStartedException : Exception
    {
        public ChallengeHasNotStartedException() : base("The challenge has not started yet.")
        {
            
        }
    }
}
