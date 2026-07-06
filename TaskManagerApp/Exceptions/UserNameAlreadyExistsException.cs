namespace TaskManagerApp.Exceptions
{
    public class UserNameAlreadyExistsException : Exception
    {
        public UserNameAlreadyExistsException() : base("User with this username already exists.")
        {
        }
    }
}
