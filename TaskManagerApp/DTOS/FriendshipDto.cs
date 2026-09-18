namespace TaskManagerApp.DTOS
{
    public class FriendshipDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public string SenderUsername { get; set; } = "";

        public int ReceiverId { get; set; }

        public string ReceiverUsername { get; set; } = "";

        public string Status { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}