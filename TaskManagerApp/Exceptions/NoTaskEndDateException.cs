namespace TaskManagerApp.Exceptions
{
    public class NoTaskEndDateException : Exception
    {
        public NoTaskEndDateException() : base("The task must have an end date!")
        {
             
        }
    }
}
