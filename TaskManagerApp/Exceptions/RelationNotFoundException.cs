namespace TaskManagerApp.Exceptions
{
    public class RelationNotFoundException : Exception
    {
        public RelationNotFoundException() : base("The relation between these users was not found!")
        { }
    }
}
