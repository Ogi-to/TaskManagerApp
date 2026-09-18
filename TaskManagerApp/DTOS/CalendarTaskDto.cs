using TaskManagerApp.Data.Models;

namespace TaskManagerApp.DTOS
{
    public class CalendarTaskDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public StateType State { get; set; }

        public int Priority { get; set; }

        public int UserId { get; set; }

        public string CreatedByUsername { get; set; } = "";

        public int CalendarId { get; set; }

        public List<string> Categories { get; set; } = new();
    }
}