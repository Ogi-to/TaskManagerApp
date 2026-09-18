using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IUserService
    {
        public Task RegisterUser(RegisterUserDto registerUserDto);
        public Task LogInUser(LoginUserDto loginUserDto, string? ipAddress);
        public Task<UserDto> UseTheSendCodeForLogin(VerifyEmailDto verifyEmailDto);
        public Task<UserDto> GetUserById(int id);
        public Task DeleteAccount(int userId);
        public Task VerifyEmail(VerifyEmailDto verifyEmailDto);
        public Task SendReminderForTasksEmail();
        public Task SendReminderForStreakEmail();
        public Task UpdateStreak(UpdateUserDto user);
        public Task UpdateRank(UpdateUserDto user);
        public Task UpdatePoints(int userId, int points);
        public Task SendFriendRequest(int initiatorId, int relatedUserId);
        public Task AnswerToSentRequest(int relatedUserId, int userInitiatorId, RelationStatus relationStatus);
        public Task<List<UsersRelationsDto>> GetUnansweredRelationReceivedByUserIdAsync(int relatedUserId);
        public Task UpdateUserRelation(int initiatorId, int relatedUserId, RelationType relationType);
        public Task DeleteUserRelationByMoreThanAMonth();
        public Task UpdateReminderSettings(ReminderSettingsDto reminderSettingsDto);
        public Task ReSendVerificationCode(string email);
        public Task<List<TaskDto>> GetAllFinishedTasksByUserId(int userId);
    }
}
