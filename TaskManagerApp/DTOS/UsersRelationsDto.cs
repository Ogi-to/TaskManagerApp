using TaskManagerApp.Data.Models;

namespace TaskManagerApp.DTOS
{
    public class UsersRelationsDto
    {
        public UserDto Initiator { get; set; }
        public UserDto RelatedUser { get; set; }
        public RelationType? RelationType { get; set; }

        public RelationStatus RelationStatus { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? TimeOfAction { get; set; }
    }
}
