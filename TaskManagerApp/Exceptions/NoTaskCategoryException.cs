namespace TaskManagerApp.Exceptions
{
    public class NoTaskCategoryException : Exception
    {
        public NoTaskCategoryException() : base("The task must have at least one category!")
        {
        }
    }
}
