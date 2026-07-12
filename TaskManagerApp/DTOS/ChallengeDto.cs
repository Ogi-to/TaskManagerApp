namespace TaskManagerApp.DTOS
{
    public class ChallengeDto
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public int DurationDays { get; set; }
        public int Level { get; set; }
        public int CategoryId { get; set; }
        public int? Trophy { get; set; }
        public int Points { get; set; }
        public string Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
