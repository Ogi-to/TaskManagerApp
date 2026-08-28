using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class LoginAttempt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string IpAddress { get; set; } = string.Empty;

        public int FailedAttempts { get; set; }
        public DateTime? BlockedUntil{  get; set; }
        public DateTime? LastAttempt { get; set; }

    }
}
