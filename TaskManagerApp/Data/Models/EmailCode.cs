namespace TaskManagerApp.Data.Models
{
    public class EmailCode
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
