namespace TaskManagerApp.DTOS
{
    public class CalendarMemberDto
    {
        public int UserId { get; set; }

        public string Username { get; set; } = "";

        public string Email { get; set; } = "";

        public DateTime JoinedAt { get; set; }
    }
}