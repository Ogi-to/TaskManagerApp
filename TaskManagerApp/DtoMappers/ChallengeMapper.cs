using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.DtoMappers
{
    public static class ChallengeMapper
    {
        public static ChallengeDto ToDto(this Challenge challenge)
        {
            return new ChallengeDto
            {
                Id = challenge.Id,
                Title = challenge.Title,
                DurationDays = challenge.DurationDays,
                Level = challenge.Level,
                CategoryId = challenge.CategoryId,
                Trophy = challenge.Trophy,
                Points = challenge.Points,
                Description = challenge.Description,
                StartDate = challenge.StartDate,
                EndDate = challenge.EndDate
            };
        }
    }
}
