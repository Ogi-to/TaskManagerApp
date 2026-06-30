namespace TaskManagerApp.Exceptions
{
    public class EmailorPasswordNotFoundException : Exception
    {
        public EmailorPasswordNotFoundException() : base("Wrong email or password.")
        { }
    }
}
