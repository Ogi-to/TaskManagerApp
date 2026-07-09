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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserStatsService _userStatsService;
        public TaskItemService(ITaskItemRepository repository, IUserRepository userRepository, IUserService userService, ICategoryRepository categoryRepository, IUserStatsService userStatsService)
        {
            _taskItemRepository = repository;
            _userRepository = userRepository;
            _userService = userService;
            _categoryRepository = categoryRepository;
            _userStatsService = userStatsService;
        }
        public async Task AddTaskAsync(TaskItem task, User user, List<int> categoryIds)
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
                if (task.EndDate <= DateTime.UtcNow)
                {
                    throw new IncorrectTaskEndDateException();
                }
            }

            var categories = await _categoryRepository.GetByIdsAsync(categoryIds);

            if (categories.Count != categoryIds.Count)
            {
                throw new Exception("These categories do not exist.");
            }

   

            var existingUser = await _userRepository.GetAsync(user.Id);
            if (existingUser == null) {
                throw new UserNotFoundException(user.Id);
            }
            var finalTask = new TaskItem
            {
                Name = task.Name,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                UserId = existingUser.Id,
            };
            foreach (var categoryId in categoryIds)
            {
                finalTask.TasksCategories.Add(new TasksCategories
                {
                    CategoryId = categoryId
                });
            }


            await _taskItemRepository.AddTaskAsync(finalTask);


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
                existing.UserId == updated.UserId;

            if (scalarFieldsEqual == false)
            {
                return false;
            }

            var existingCategoryIds = existing.TasksCategories.Select(tc => tc.CategoryId).ToList();
            var newCategoryIds = updated.TasksCategories.Select(tc => tc.CategoryId).ToList();
            bool categoriesEqual = existingCategoryIds.Count == newCategoryIds.Count && !existingCategoryIds.Except(newCategoryIds).Any();

            return  categoriesEqual;
        }

        public async Task CompleteTask(TaskItem task)
        {
            var existingTask = await _taskItemRepository.GetAsync(task.Id);
            if (existingTask == null)
            {
                throw new TaskItemNotFoundException();
            }

            if(existingTask.State == StateType.Completed)
            {
                throw new TaskAlreadyCompletedException();
            }
            if (existingTask.State == StateType.Overdue)
            {
                throw new TaskAlreadyOverdueException();
            }

            

            existingTask.User.Points += 200;
            await _userService.UpdatePoints(existingTask.User);
            await _userService.UpdateStreak(existingTask.User);
            await _userService.UpdateRank(existingTask.User);

            var userStats = await _userStatsService.ShowUserStatsByIdAsync(existingTask.UserId);
            userStats.TasksCompleted += 1;



            await _userStatsService.UpdateUserStatsByIdAsync(existingTask.UserId);

            await _taskItemRepository.CompleteTask(existingTask);


        }

        public async Task MarkOverdueTasksAsync()
        {
            var overdueTasks = await _taskItemRepository.GetTasksPastDueAsync(DateTime.UtcNow);

            foreach (var task in overdueTasks)
            {
                task.State = StateType.Overdue;
                await _taskItemRepository.UpdateAsync(task);
            }
        }
    }
}

