namespace TaskManagerApp.Exceptions
{
    public class TooManyFailedLoginAttemptsException : Exception
    {
        public TooManyFailedLoginAttemptsException(double dateTime) : base($"Too many failed login attempts. Please try again after {dateTime} minutes.")
        {
            
        }
    }
}
