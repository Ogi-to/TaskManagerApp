namespace TaskManagerApp.Exceptions
{
    public class IncorrectTaskEndDateException : Exception
    {
        public IncorrectTaskEndDateException() : base("The task end date is incorrect!")
        {
        }
    }
}
