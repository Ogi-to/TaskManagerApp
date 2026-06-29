using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ITaskItemRepository
    {
   
        public TaskItem Get(int id);

        public void Update(TaskItem item);

        public void Delete(TaskItem item);

        public TaskItem AddTask(TaskItem item);

        public void AssignToUser(TaskItem task , User user);

        public List<TaskItem> GetAll();
    }
}
