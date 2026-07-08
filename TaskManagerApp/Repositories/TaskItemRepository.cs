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
                TaskId = taskToModify.Id,
                State = StateType.NotStarted
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem item)
        {
            // the delete behaviour in "on model creating" is restrict, so the relationships have to be updated manually
            _context.UsersTasks.RemoveRange(item.UsersTasks);
            _context.TasksCategories.RemoveRange(item.TasksCategories);
            _context.TaskItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem> GetAsync(int id)
        {
            return await _context.TaskItems
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
                    .Include(t => t.TasksCategories)
                        .ThenInclude(tc => tc.Category)
                    .Include(t => t.UsersTasks)
                        .ThenInclude(ut => ut.User)
                     .Include(t => t.UsersTasks)
                        .ThenInclude(s => s.State)
                    .OrderBy(t => t.EndDate)
                    .ToListAsync();
        }


        public async Task UpdateAsync(UsersTasks item)
        {
            var taskToModify = await _context.UsersTasks.Include(t => t.Task).FirstOrDefaultAsync(t => t.TaskId == item.TaskId && t.UserId == item.UserId); 

            taskToModify.Task.Name = item.Task.Name;
            taskToModify.Task.Description = item.Task.Description;
            taskToModify.State = item.State;
            taskToModify.Task.StartDate = item.Task.StartDate;
            taskToModify.Task.EndDate = item.Task.EndDate;

            //manually because the delete behaviour is set to strict
            //for UserTasks
            var currentUserIds = taskToModify.Task.UsersTasks.Select(ut => ut.UserId).ToList();
            var newUserIds = item.Task.UsersTasks.Select(ut => ut.UserId).ToList();

            List<UsersTasks> userJoinsToRemove = taskToModify.Task.UsersTasks.Where(ut => !newUserIds.Contains( ut.UserId)).ToList(); // tezi koito ne se sydurjat w nowite
            foreach (var ut in userJoinsToRemove)
            {
                taskToModify.Task.UsersTasks.Remove(ut);
            }

            List<UsersTasks> userJoinsToAdd = item.Task.UsersTasks.Where(ut => !currentUserIds.Contains(ut.UserId)).ToList(); // tezi koito ne se sydyrjat w segashnite
            foreach (var ut in userJoinsToAdd)
            {
                taskToModify.Task.UsersTasks.Add(new UsersTasks
                {
                    UserId = ut.UserId,
                    TaskId = taskToModify.Task.Id
                });
            }

            //for TasksCategories
            var currentCategoryIds = taskToModify.Task.TasksCategories.Select(tc => tc.CategoryId).ToList();
            var newCategoryIds = item.Task.TasksCategories.Select(tc => tc.CategoryId).ToList();

            List<TasksCategories> categoryJoinsToRemove = taskToModify.Task.TasksCategories.Where(ut => !newCategoryIds.Contains(ut.CategoryId)).ToList(); // tezi koito ne se sydurjat w nowite
            foreach (var ut in categoryJoinsToRemove)
            {
                taskToModify.Task.TasksCategories.Remove(ut);
            }

            List<TasksCategories> categoryJoinsToAdd = item.Task.TasksCategories.Where(ut => !currentCategoryIds.Contains(ut.CategoryId)).ToList(); // tezi koito ne se sydyrjat w segashnite
            foreach (var ut in categoryJoinsToAdd)
            {
                taskToModify.Task.TasksCategories.Add(new TasksCategories
                {
                    CategoryId = ut.CategoryId,
                    TaskId = taskToModify.Task.Id
                });
            }


            await _context.SaveChangesAsync();
        }

        public async Task CompleteTask(UsersTasks item)
        {
            var task = await _context.UsersTasks.FirstOrDefaultAsync(t => t.TaskId == item.TaskId && t.UserId == item.UserId);
            task.State = StateType.Completed;


            await _context.SaveChangesAsync();
        }
    }
}
