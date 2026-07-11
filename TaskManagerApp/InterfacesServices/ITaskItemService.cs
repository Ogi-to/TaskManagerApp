using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.InterfacesServices
{
    public interface ITaskItemService
    {
        public Task AddTaskAsync(AddTaskDto task);

        public Task DeleteTaskAsync(int taskId);
        public Task<TaskDto> GetTaskAsync(int taskId);
        public Task<List<TaskDto>> ShowAllTasksByUserIdAsync(int userId);

        public Task UpdateTaskAsync(UpdateTaskDto task);

        public Task CompleteTask(int taskId);
        public Task MarkOverdueTasksAsync();
        public Task DeleteOverdueTasksMoreThanDay();


    }
}

