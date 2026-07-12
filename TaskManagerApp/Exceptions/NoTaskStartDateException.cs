namespace TaskManagerApp.Exceptions
{
    public class NoTaskStartDateException : Exception
    {
        public NoTaskStartDateException() : base("The task must have a start date!")
        {
        }
    }
}
