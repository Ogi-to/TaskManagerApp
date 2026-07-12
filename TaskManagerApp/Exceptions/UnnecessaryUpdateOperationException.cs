namespace TaskManagerApp.Exceptions
{
    public class UnnecessaryUpdateOperationException : Exception
    {
        public UnnecessaryUpdateOperationException() : base("No changes are detected. Update operation is not necessary.")
        {
        }

    }
}
