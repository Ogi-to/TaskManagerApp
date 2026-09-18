namespace TaskManagerApp.Exceptions
{
    public class UserStatsAlreadyExistException : Exception
    {
    public UserStatsAlreadyExistException() : base("User stats for the given user already exist.")
    {
    }
    }
}
