namespace TaskManagerApp.Exceptions
{
    public class ChallengeAlreadyCompletedException : Exception
    {
    public ChallengeAlreadyCompletedException() : base("Challenge has already been completed by the user.")
        { }
    }
}
