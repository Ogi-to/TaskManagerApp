namespace TaskManagerApp.DTOS
{
    public class UpdateUserDto
    {
        public int Streak { get; set; }
        public int Points { get; set; }
        public int RankId { get; set; }
        public DateTime? LastActive { get; set; }
    }
}