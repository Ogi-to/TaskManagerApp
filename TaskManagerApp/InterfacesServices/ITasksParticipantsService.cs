using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface ITasksParticipantsService
    {
        public Task AnswerInviteForTask(int friendId, int taskId, Status status);
        public Task<List<TasksParticipantsDto>> GetByUserId(int userId);
        public Task<List<TasksParticipantsDto>> GetByTaskId(int taskId);
        public Task<TasksParticipantsDto> GetByBoth(int taskId, int userId);
        public Task DeleteByBoth(int taskId, int userId);
        public Task InviteFriendsToTask(int ownerId, int friendId, int taskId);
    }
}
