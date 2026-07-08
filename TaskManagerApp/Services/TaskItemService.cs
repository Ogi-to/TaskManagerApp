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
        private readonly IUserService _userService;
        public TaskItemService(ITaskItemRepository repository, IUserRepository userRepository, IUserService userService)
        {
            _taskItemRepository = repository;
            _userRepository = userRepository;
            _userService = userService;
        }
        public async Task AddTaskAsync(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new EmptyTaskNameException();
            }
            if (task.StartDate == null)
            {
                throw new NoTaskStartDateException();
            }
            //if (task.EndDate == null)
            //{
            //throw new NoTaskEndDateException();
            //}
            if (task.EndDate != null)
            {
                if (task.EndDate < task.StartDate)
                {
                    throw new IncorrectTaskEndDateException();
                }
                if (task.EndDate <= DateTime.Now)
                {
                    throw new IncorrectTaskEndDateException();
                }
            }

            if (!task.TasksCategories.Any())
            {
                throw new NoTaskCategoryException();
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
                throw new UserAlreadyHasThisTaskException();
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
                throw new UserDoesntHaveTasksException();
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
                throw new UnnecessaryUpdateOperationException();
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
                existing.State == updated.State;

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

        public async Task CompleteTask(TaskItem task, User user)
        {
            UsersTasks usersTasks = new UsersTasks();
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

            if(!existingTask.UsersTasks.Any(ut => ut.UserId == existingUser.Id))
            {
                throw new UserNotAssignedToTaskException();
            }
            if()
            {
                throw new TaskAlreadyCompletedException();
            }

            existingUser.Points += 200;
            await _userService.UpdatePoints(existingUser);
            await _userService.UpdateStreak(existingUser);
            await _userService.UpdateRank(existingUser);

            await _taskItemRepository.CompleteTask(existingTask);


        }
    }
}

