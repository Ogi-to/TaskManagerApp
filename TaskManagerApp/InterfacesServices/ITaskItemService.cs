using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.InterfacesServices
{
    public interface ITaskItemService
    {
        public Task AddTaskAsync(TaskItem task, int userId, List<int> categoryIds);

        public Task DeleteTaskAsync(int taskId);
        public Task<TaskItem> ShowTaskAsync(int taskId);
        public Task<List<TaskItem>> ShowAllTasksByUserIdAsync(int userId);

        public Task UpdateTaskAsync(TaskItem task);

        public Task CompleteTask(TaskItem task);
        public Task MarkOverdueTasksAsync();
        public Task DeleteOverdueTasksMoreThanDay();


    }
}

