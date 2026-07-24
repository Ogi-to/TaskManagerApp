using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ITaskItemRepository
    {

        public Task<TaskItem> GetAsync(int id);

        public Task UpdateAsync(TaskItem item);

        public Task DeleteAsync(int itemId);

        public Task AddTaskAsync(TaskItem item);


        public Task<List<TaskItem>> GetAllAsync();
        public Task<List<TaskItem>> GetAllByUserAsync(int userId);

        public Task<List<TaskItem>> GetAllAboutToStartAsync();
        public Task SaveChanges();
        public Task<List<TaskItem>> GetCompletedTasksTodayByUser(int userId);
        public Task CompleteTask(int taskId);

        public Task<List<TaskItem>> GetTasksPastDueAsync(DateTime utcNow);
        public Task DeleteOverdueTaskMoreThanDay(DateTime utcNow);
    }
}
