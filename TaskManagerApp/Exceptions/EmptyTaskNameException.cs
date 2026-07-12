namespace TaskManagerApp.Exceptions
{
    public class EmptyTaskNameException : Exception
    {
        public EmptyTaskNameException() : base("The task must have a name!")
        {
            
        }
    }
}
