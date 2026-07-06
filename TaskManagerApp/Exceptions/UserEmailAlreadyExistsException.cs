namespace TaskManagerApp.Exceptions
{
    public class UserEmailAlreadyExistsException : Exception
    {
        public UserEmailAlreadyExistsException() : base("User with this email already exists.")
        {
            
        }
    }
}
