using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IUserRepository _userRepository;
        public TaskItemService(ITaskItemRepository repository, IUserRepository userRepository)
        {
            _taskItemRepository = repository;
            _userRepository = userRepository;

        }
        public async Task AddTaskAsync(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new Exception("Task name cannot be empty.");
            }
            if (task.StartDate == null)
            {
                throw new Exception("Task must have a start date.");
            }
            if (task.EndDate == null)
            {
                throw new Exception("Task must have an end date.");
            }
            if (task.EndDate < task.StartDate)
            {
                throw new Exception("Task end date cannot be before the start date.");
            }
            if (task.EndDate <= DateTime.Now)
            {
                throw new Exception("Task end date must be in the future.");
            }
            

            await _taskItemRepository.AddTaskAsync(task);


        }

        public async Task AddUserAsync(TaskItem task, User user)
        {
            var existingTask = await _taskItemRepository.GetAsync(task.Id);
            if (existingTask == null)
            {
                throw new TaskItemNotFoundException();
            }

            var existingUser = await _userRepository.GetAsync(user.Id);
            if (existingUser == null)
            {
                throw new UserNotFoundException(user.Id);
            }

            if (existingTask.UsersTasks.Any(ut => ut.UserId == existingUser.Id))
            {
                throw new Exception($"User with ID {existingUser.Id} is already assigned to the task with ID {existingTask.Id}.");
            }

            await _taskItemRepository.AssignToUserAsync(existingTask, existingUser);

        }

        public async Task DeleteTaskAsync(int taskId)
        {
            var taskToDelete = await _taskItemRepository.GetAsync(taskId);

            if (taskToDelete == null)
            {
                throw new TaskItemNotFoundException();
            }
             
            await _taskItemRepository.DeleteAsync(taskToDelete);

        }

        public async Task<List<TaskItem>> ShowAllTasksByUserIdAsync(int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            var tasks = await _taskItemRepository.GetAllByUserAsync(userId);
            if (!tasks.Any())
            {
                throw new Exception($"No tasks found for user with ID {userId}.");
            }

            return tasks;
        }

        public async Task<TaskItem> ShowTaskAsync(int taskId)
        {
            var existingTask = await _taskItemRepository.GetAsync(taskId);
            if (existingTask == null)
            {
                throw new TaskItemNotFoundException();
            }

            return existingTask;
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            var existingTask = await _taskItemRepository.GetAsync(task.Id);
            if (existingTask == null)
            {
                throw new TaskItemNotFoundException();
            }

            if (AreTasksEqual(existingTask, task))
            {
                throw new Exception("No changes are detected. Update operation is not necessary.");
            }

            await _taskItemRepository.UpdateAsync(task);
        }
            
        //used in updateTaskAsync
        private static bool AreTasksEqual(TaskItem existing,TaskItem updated )
        {
            bool scalarFieldsEqual = existing.Name == updated.Name &&
                existing.Description == updated.Description &&
                existing.StartDate == updated.StartDate &&
                existing.EndDate == updated.EndDate &&
                existing.StateId == updated.StateId;

            if (scalarFieldsEqual == false)
            {
                return false;
            }
             
            var existingUserIds = existing.UsersTasks.Select(ut => ut.UserId).ToList();
            var newUserIds = updated.UsersTasks.Select(ut => ut.UserId).ToList();
            bool usersEqual = existingUserIds.Count == newUserIds.Count && !existingUserIds.Except(newUserIds).Any();

            var existingCategoryIds = existing.TasksCategories.Select(tc => tc.CategoryId).ToList();
            var newCategoryIds = updated.TasksCategories.Select(tc => tc.CategoryId).ToList();
            bool categoriesEqual = existingCategoryIds.Count == newCategoryIds.Count && !existingCategoryIds.Except(newCategoryIds).Any();

            return usersEqual && categoriesEqual;
        }
    }
}

