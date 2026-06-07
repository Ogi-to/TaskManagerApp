using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ITaskItemRepository
    {
        public TaskItem Get();
        public TaskItem Get(TaskItem item);

        public TaskItem Update(TaskItem item);

        public TaskItem Delete(TaskItem item);

        public TaskItem AddTask(TaskItem item);

        public TaskItem AssignToUser();

        public List<TaskItem> GetAll();
    }
}
