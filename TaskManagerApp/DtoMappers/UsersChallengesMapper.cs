using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.DtoMappers
{
    public static class UsersChallengesMapper
    {
        public static UsersChallengesDto ToDto(this UsersChallenges userChallenge)
        {
            return new UsersChallengesDto
            {
                State = userChallenge.State,
                Challenge = userChallenge.Challenge.ToDto()
            };
        }
    }
}
