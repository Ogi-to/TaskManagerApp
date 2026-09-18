using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Data.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }

        // Потребителят, който изпраща поканата
        public int SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; } = null!;


        // Потребителят, който получава поканата
        public int ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public User Receiver { get; set; } = null!;


        // Статус на приятелството
        public FriendshipStatus Status { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }


    public enum FriendshipStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }
}