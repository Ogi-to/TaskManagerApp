using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ITaskItemRepository
    {
   
        public TaskItem Get(TaskItem item);

        public void Update(TaskItem item);

        public void Delete(TaskItem item);

        public void AddTask(TaskItem item);

        public void AssignToUser(TaskItem task , User user);

        public List<TaskItem> GetAll();
    }
}
