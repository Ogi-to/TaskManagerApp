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
            item.State = StateType.NotStarted;
            _context.TaskItems.Add(item);
            await _context.SaveChangesAsync();
            
        }

        public async Task DeleteAsync(int itemId)
        {
            var item = await _context.TaskItems.FindAsync(itemId);
            // the delete behaviour in "on model creating" is restrict, so the relationships have to be updated manually
            _context.TasksCategories.RemoveRange(item.TasksCategories);
            _context.TaskItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem> GetAsync(int id)
        {
            return await _context.TaskItems
                   .Include(t => t.TasksCategories)
                       .ThenInclude(tc => tc.Category)
                   .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<List<TaskItem>> GetAllByUserAsync(int userId)
        {

                return await _context.TaskItems
           .Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<List<TaskItem>> GetAllAboutToStartAsync()
        {
            var now = DateTime.UtcNow;

            var tasks = await _context.TaskItems
                .Include(t => t.User)
                .Where(t => t.User.ReminderStartBefore > 0).Where(t => t.State == StateType.NotStarted).Where(t => t.StartDate >= now).
                Where(t => t.StartDate <= now.AddMinutes(t.User.ReminderStartBefore))
                .ToListAsync();
            return tasks;
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
                return await _context.TaskItems
           .Include(t => t.TasksCategories)
               .ThenInclude(tc => tc.Category)
           .ToListAsync();
        }


        public async Task UpdateAsync(TaskItem item)
        {
            var taskToModify = await _context.TaskItems
    .Include(t => t.TasksCategories)
    .FirstOrDefaultAsync(t => t.Id == item.Id);

            taskToModify.Name = item.Name;
            taskToModify.Description = item.Description;
            taskToModify.State = item.State;
            taskToModify.StartDate = item.StartDate;
            taskToModify.EndDate = item.EndDate;

            //for TasksCategories
            var currentCategoryIds = taskToModify.TasksCategories.Select(tc => tc.CategoryId).ToList();
            var newCategoryIds = item.TasksCategories.Select(tc => tc.CategoryId).ToList();

            List<TasksCategories> categoryJoinsToRemove = taskToModify.TasksCategories.Where(ut => !newCategoryIds.Contains(ut.CategoryId)).ToList(); // tezi koito ne se sydurjat w nowite
            foreach (var ut in categoryJoinsToRemove)
            {
                taskToModify.TasksCategories.Remove(ut);
            }

            List<TasksCategories> categoryJoinsToAdd = item.TasksCategories.Where(ut => !currentCategoryIds.Contains(ut.CategoryId)).ToList(); // tezi koito ne se sydyrjat w segashnite
            foreach (var ut in categoryJoinsToAdd)
            {
                taskToModify.TasksCategories.Add(new TasksCategories
                {
                    CategoryId = ut.CategoryId,
                    TaskId = taskToModify.Id
                });
            }


            await _context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CompleteTask(int taskId)
        {
            var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == taskId);
            task.State = StateType.Completed;


            await _context.SaveChangesAsync();
        }

        public async Task<List<TaskItem>> GetTasksPastDueAsync(DateTime utcNow)
        {
            return await _context.TaskItems
                .Where(t => t.EndDate < utcNow &&
                            t.State != StateType.Completed &&
                            t.State != StateType.Overdue)
                .ToListAsync();
        }

        public async Task DeleteOverdueTaskMoreThanDay(DateTime utcNow)
        {
            var overdueTasks = await _context.TaskItems.Where(t => t.State == StateType.Overdue && t.EndDate < utcNow.AddDays(-1))
                .ToListAsync();
            _context.TaskItems.RemoveRange(overdueTasks);
            await _context.SaveChangesAsync();
        }


    
    }
}
