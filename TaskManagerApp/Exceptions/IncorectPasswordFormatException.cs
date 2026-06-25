namespace TaskManagerApp.Exceptions
{
    public class IncorectPasswordFormatException : Exception
    {
        public IncorectPasswordFormatException(string password) : base($"The password '{password}' is not in a valid format.")
        {
        }
    }
}
