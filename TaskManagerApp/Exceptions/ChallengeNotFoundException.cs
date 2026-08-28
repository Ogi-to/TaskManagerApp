namespace TaskManagerApp.Exceptions
{
    public class ChallengeNotFoundException : Exception
    {
        public ChallengeNotFoundException() : base("Challenge not found.")
        {
            
        }
    }
}
