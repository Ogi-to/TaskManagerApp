namespace TaskManagerApp.Data.Models
{
    public class UsersChallenges
    {
        public int UserId { get; set; }

        public int ChallengeId { get; set; }

        public User User { get; set; }
        public Challenge Challenge { get; set; }

        public int StateId { get; set; }
        public State State { get; set; }


    }
}
