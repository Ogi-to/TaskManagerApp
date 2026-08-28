using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IEmailService
    {
        public Task<bool> VerifyEmail(string email, string code);
        public Task SendVerificationCode(string email);
        public Task SendReminderTaskEmail(string username, string email, TaskItem taskItem);
        public Task SendEmailForNewChallenges(string username, string email, List<Challenge> challenges);
        public Task SendReminderForStreakEmail(User user);
        public Task SendFriendRequestToTheRelatedUserEmail(UserDto initiator, UserDto receiver);
        public Task AnswerFriendRequestInformInitiatorEmail(UsersRelationsDto usersRelationsDto);
        public Task UpdateRelationShipStatusInformBothEmail(string email, string initiatorName, string reciverName, RelationType relationType);
        public Task InviteFriendToTaskEmail(string friendEmail, string friendName, string ownerName, string taskName, DateTime taskStartDate);
        public Task AnswerTaskInviteInforOwner(string ownerEmail, string ownerName, string friendName, string taskName, DateTime taskStartDate, Status status);
        public Task DeleteCodes();
    }
}
