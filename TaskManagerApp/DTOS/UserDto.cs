namespace TaskManagerApp.DTOS
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int Streak { get; set; }

        public int Points { get; set; }

        public int RankId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastActive { get; set; }

        public string UserCode { get; set; }
        public int ReminderStartBefore { get; set; }
        public int ReminderInterval { get; set; }
    }
}
