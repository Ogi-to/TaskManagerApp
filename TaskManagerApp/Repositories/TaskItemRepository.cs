using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private TaskManagerDbContext _context;
        public TaskItemRepository(TaskManagerDbContext context)
        {
            _context = context;
        }
        public TaskItem AddTask(TaskItem item)
        {
            _context.TaskItems.Add(item);
            _context.SaveChanges();
            return item;
        }

        public void AssignToUser(TaskItem task, User user)
        {
            TaskItem taskToModify = _context.TaskItems.Where(t => t.Id == task.Id).FirstOrDefault();
            taskToModify.Users.Add(user);
            _context.SaveChanges();
        }

        public void Delete(TaskItem item)
        {
            _context.TaskItems.Remove(item);
            _context.SaveChanges();
        }

        public TaskItem Get(int id)
        {
            return _context.TaskItems.Where(t => t.Id == id).Include(t => t.State).Include(t => t.Categories).Include(t => t.Users).FirstOrDefault();
        }

        public List<TaskItem> GetAll()
        {
            return _context.TaskItems.Include(t => t.State).Include(t => t.Categories).Include(t => t.Users).OrderBy(t => t.EndDate).ToList();
        }

        public void Update(TaskItem item)
        {
            TaskItem taskToModify = _context.TaskItems.Where(t => t.Id == item.Id).FirstOrDefault();
            taskToModify.Name = item.Name;
            taskToModify.Description = item.Description;
            taskToModify.StateId = item.StateId;
            taskToModify.State = item.State;
            taskToModify.Categories = item.Categories;
            taskToModify.Users = item.Users;
            taskToModify.StartDate = item.StartDate;
            taskToModify.EndDate = item.EndDate;
            _context.SaveChanges();
        }

    }
}
