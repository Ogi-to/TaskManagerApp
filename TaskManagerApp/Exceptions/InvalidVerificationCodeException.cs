namespace TaskManagerApp.Exceptions
{
    public class InvalidVerificationCodeException : Exception
    {
        public InvalidVerificationCodeException() : base("The verification code provided is invalid or has expired.")
        {
            
        }
    }
}
