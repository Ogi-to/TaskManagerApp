using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTOS
{
    public class CreateCalendarDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        public string Color { get; set; } = "blue";

        public List<int> FriendIds { get; set; } = new();
    }
}