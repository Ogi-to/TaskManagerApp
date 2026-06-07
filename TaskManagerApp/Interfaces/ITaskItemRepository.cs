using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ITaskItemRepository
    {
        public TaskItem Get();

        public TaskItem Update(TaskItem item);
    }
}
