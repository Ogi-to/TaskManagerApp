namespace TaskManagerApp.Exceptions
{
    public class IncorectEmailFormatException : Exception
    {
        public IncorectEmailFormatException(string email) : base($"The email '{email}' is not in a valid format.")
        {
        }
    }
}
