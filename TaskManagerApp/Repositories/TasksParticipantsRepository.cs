using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesRepositories;

namespace TaskManagerApp.Repositories
{
    public class TasksParticipantsRepository : ITasksParticipantsRepository
    {
        private readonly TaskManagerDbContext _context;
        public TasksParticipantsRepository(TaskManagerDbContext taskManagerDbContext)
        {
            _context = taskManagerDbContext;
        }
        public async Task DeleteByBoth(int taskId, int userId)
        {
            TasksParticipants tasksParticipants = await _context.TasksParticipants.Where(tp => tp.TaskId == taskId && tp.UserId == userId).FirstOrDefaultAsync();
            _context.TasksParticipants.Remove(tasksParticipants);
            await _context.SaveChangesAsync();

        }

        public async Task Add(TasksParticipants tasksParticipants)
        {
            tasksParticipants.Status = Status.Pending;
            _context.Add(tasksParticipants);
            await _context.SaveChangesAsync(); 
        }

        public async Task<List<TasksParticipants>> GetAllPending()
        {
            List<TasksParticipants> tasksParticipants = await _context.TasksParticipants.Where(tp => tp.Status == Status.Pending).ToListAsync();
            return tasksParticipants;
        }

        public async Task<TasksParticipants> GetByBoth(int taskId, int userId)
        {
            return await _context.TasksParticipants
            .Include(tp => tp.User)
            .Include(tp => tp.TaskItem)
                .ThenInclude(t => t.TasksCategories)
                    .ThenInclude(tc => tc.Category)
            .Where(tp => tp.UserId == userId && tp.TaskId == taskId)
            .FirstOrDefaultAsync();
        }

        public async Task<List<TaskItem>> GetAllFinishedSharedTasksByUserId(int userId)
        {
            return await _context.TasksParticipants.Where(tp => tp.UserId == userId && 
            tp.Status == Status.Accepted && tp.TaskItem.State == StateType.Completed).Select(tp => tp.TaskItem).Include(t => t.TasksCategories)
            .ThenInclude(tc => tc.Category).ToListAsync();
               
        }

        public async Task<List<TasksParticipants>> GetByTaskId(int taskId)
        {
            return await _context.TasksParticipants
            .Include(tp => tp.User)
            .Include(tp => tp.TaskItem)
                .ThenInclude(t => t.TasksCategories)
                    .ThenInclude(tc => tc.Category)
            .Where(tp => tp.TaskId == taskId)
            .ToListAsync();
        }

        public async Task<List<TasksParticipants>> GetJoinedInTaskByTaskId(int taskId)
        {
            List<TasksParticipants> tasksParticipants = await _context.TasksParticipants.Where(tp => tp.Status == Status.Accepted && tp.TaskId == taskId).ToListAsync();
            return tasksParticipants;
        }

        public async Task<List<TasksParticipants>> GetByUserId(int userId)
        {
             return await _context.TasksParticipants
             .Include(tp => tp.TaskItem)
                 .ThenInclude(t => t.TasksCategories)
                     .ThenInclude(tc => tc.Category)
             .Where(tp => tp.UserId == userId)
             .ToListAsync();
        }

        public async Task Update(TasksParticipants tasksParticipants)
        {
            TasksParticipants tasksParticipantsToModify = await _context.TasksParticipants.Where(tp => tp.TaskId == tasksParticipants.TaskId && 
            tp.UserId == tasksParticipants.UserId).FirstOrDefaultAsync();

            tasksParticipantsToModify.Status = tasksParticipants.Status;

            _context.SaveChanges();
        }
    }
}
