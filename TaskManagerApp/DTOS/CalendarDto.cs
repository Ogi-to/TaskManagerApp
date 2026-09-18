namespace TaskManagerApp.DTOS
{
    public class CalendarDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Color { get; set; } = "blue";

        public int OwnerId { get; set; }

        public string OwnerUsername { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public List<CalendarMemberDto> Members { get; set; } = new();

        public List<CalendarTaskDto> Tasks { get; set; } = new();
    }
}