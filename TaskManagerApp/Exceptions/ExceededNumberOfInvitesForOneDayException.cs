namespace TaskManagerApp.Exceptions
{
    public class ExceededNumberOfInvitesForOneDayException : Exception
    {
        public ExceededNumberOfInvitesForOneDayException(int numberOfInvites) : base($"You can not send more than {numberOfInvites} invites in one day!")
        {
            
        }
    }
}
