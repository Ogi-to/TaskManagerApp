namespace TaskManagerApp.Exceptions
{
    public class UserCodeNotFoundException : Exception
    {
        public UserCodeNotFoundException() : base("User code not found.")
        { }
    }
}
