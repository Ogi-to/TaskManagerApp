using TaskManagerApp.Data.Models;

namespace TaskManagerApp.DTOS
{
    public class UsersChallengesDto
    {
        public ChallengeDto Challenge { get; set; }
        public StateType  State { get; set; }
    }
}
