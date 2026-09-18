namespace TaskManagerApp.Exceptions
{
    public class NoChallengesFoundForThisCategoryException : Exception
    {
        public NoChallengesFoundForThisCategoryException() : base("No challenges found for this category.")
        {
            
        }
    }
}
