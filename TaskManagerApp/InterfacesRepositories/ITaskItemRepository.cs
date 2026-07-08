using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ITaskItemRepository
    {

        public Task<TaskItem> GetAsync(int id);

        public Task UpdateAsync(TaskItem item);

        public Task DeleteAsync(TaskItem item);

        public Task AddTaskAsync(TaskItem item);


        //public Task<List<TaskItem>> GetAllAsync();
        public Task<List<TaskItem>> GetAllByUserAsync(int userId);

        public Task CompleteTask(TaskItem item);
    }
}
