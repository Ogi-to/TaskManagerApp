using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesRepositories
{
    public interface ITasksParticipantsRepository
    {
        public Task<List<TasksParticipants>> GetByTaskId(int taskId);
        public Task<List<TasksParticipants>> GetByUserId(int userId);
        public Task<TasksParticipants> GetByBoth(int taskId, int userId);
        public Task<List<TasksParticipants>> GetAllPending();
        public Task Update(TasksParticipants tasksParticipants);
        public Task Add(TasksParticipants tasksParticipants);
        public Task DeleteByBoth(int taskId, int userId);
        public Task<List<TasksParticipants>> GetJoinedInTaskByTaskId(int taskId);
        public Task<List<TaskItem>> GetAllFinishedSharedTasksByUserId(int userId);


    }
}
