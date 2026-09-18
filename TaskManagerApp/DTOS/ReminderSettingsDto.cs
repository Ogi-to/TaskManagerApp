namespace TaskManagerApp.DTOS
{
    public class ReminderSettingsDto
    {
        public int Id { get; set; }
        public int ReminderInterval { get; set; }
        public int ReminderStartBefore { get; set; }
    }
}
