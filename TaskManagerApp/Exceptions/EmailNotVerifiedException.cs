namespace TaskManagerApp.Exceptions
{
    public class EmailNotVerifiedException : Exception
    {
        public EmailNotVerifiedException() : base("Email is not verified.") { }
    }
}
