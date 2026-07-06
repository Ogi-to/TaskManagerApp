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
        public async Task AddTaskAsync(TaskItem item)
        {
            _context.TaskItems.Add(item);
            await _context.SaveChangesAsync();
            
        }

        public async Task AssignToUserAsync(TaskItem task, User user)
        {
            var taskToModify = await _context.TaskItems.Include(t => t.UsersTasks).FirstOrDefaultAsync(t => t.Id == task.Id);

            taskToModify.UsersTasks.Add(new UsersTasks
            {
                UserId = user.Id,
                TaskId = taskToModify.Id
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem item)
        {
            _context.TaskItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem> GetAsync(int id)
        {
            return await _context.TaskItems
                   .Include(t => t.State)
                   .Include(t => t.TasksCategories)
                       .ThenInclude(tc => tc.Category)
                   .Include(t => t.UsersTasks)
                       .ThenInclude(ut => ut.User)
                   .FirstOrDefaultAsync(t => t.Id == id);
        }

        //public async Task<List<TaskItem>> GetAllAsync()
        //{
        //    return await _context.TaskItems.Include(t => t.State).Include(t => t.Categories).Include(t => t.Users).OrderBy(t => t.EndDate).ToListAsync();
        //}
        public async Task<List<TaskItem>> GetAllByUserAsync(int userId)
        {

            return await _context.TaskItems
                    .Where(t => t.UsersTasks.Any(ut => ut.UserId == userId))
                    .Include(t => t.State)
                    .Include(t => t.TasksCategories)
                        .ThenInclude(tc => tc.Category)
                    .Include(t => t.UsersTasks)
                        .ThenInclude(ut => ut.User)
                    .OrderBy(t => t.EndDate)
                    .ToListAsync();
        }


        public async Task UpdateAsync(TaskItem item)
        {
            var taskToModify = await _context.TaskItems
            .FirstOrDefaultAsync(t => t.Id == item.Id);

            if (taskToModify == null)
            {
                return;
            }
             

            taskToModify.Name = item.Name;
            taskToModify.Description = item.Description;
            taskToModify.StateId = item.StateId;
            taskToModify.StartDate = item.StartDate;
            taskToModify.EndDate = item.EndDate;

            await _context.SaveChangesAsync();
        }
    }
}
