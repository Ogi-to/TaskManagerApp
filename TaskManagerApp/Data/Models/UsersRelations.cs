<<<<<<< Updated upstream
﻿using System.ComponentModel.DataAnnotations.Schema;
=======
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
>>>>>>> Stashed changes

namespace TaskManagerApp.Data.Models
{
    public class UsersRelations
    {
        public int InitiatorId { get; set; }

        [ForeignKey(nameof(InitiatorId))]
        public User Initiator { get; set; }

        public int RelatedUserId { get; set; }

        [ForeignKey(nameof(RelatedUserId))]
        public User RelatedUser { get; set; }

        public RelationType RelationType { get; set; }

        public RelationStatus RelationStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum RelationType
    {
        Friend,
        Blocked
    }

    public enum RelationStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
