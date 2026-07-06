using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.InterfacesServices
{
    public interface ITaskItemService
    {
        public Task AddTaskAsync(TaskItem task);
        public Task AddUserAsync(TaskItem task, User user);

        public Task DeleteTaskAsync(int taskId);
        public Task<TaskItem> ShowTaskAsync(int taskId);
        public Task<List<TaskItem>> ShowAllTasksByUserIdAsync(int userId);

        public Task UpdateTaskAsync(TaskItem task);

    }
}

