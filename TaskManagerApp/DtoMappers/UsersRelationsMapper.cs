using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.DtoMappers
{
    public static class UsersRelationsMapper
    {
        public static UsersRelationsDto ToDto(this UsersRelations relation)
        {
            return new UsersRelationsDto
            {
                Initiator = relation.Initiator.ToDto(),
                RelatedUser = relation.RelatedUser.ToDto(),
                RelationType = relation.RelationType,
                RelationStatus = relation.RelationStatus,
                CreatedAt = relation.CreatedAt,
                TimeOfAction = relation.TimeOfAction
            };
        }

        public static List<UsersRelationsDto> ToDto(this List<UsersRelations> relations)
        {
            return relations.Select(r => r.ToDto()).ToList();
        }
    }
}

