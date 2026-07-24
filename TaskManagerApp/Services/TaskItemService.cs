using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
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
        public TaskItemService(ITaskItemRepository repository, IUserRepository userRepository, IUserService userService,
        ICategoryRepository categoryRepository, IUserStatsService userStatsService)
        {
            _taskItemRepository = repository;
            _userRepository = userRepository;
            _userService = userService;
            _categoryRepository = categoryRepository;
            _userStatsService = userStatsService;
        }
        public async Task AddTaskAsync(AddTaskDto task)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new EmptyTaskNameException();
            }
            if (task.StartDate == default)
            {
                throw new NoTaskStartDateException();
            }
            //if (task.EndDate == null)
            //{
            //throw new NoTaskEndDateException();
            //}
            if (task.EndDate != default)
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

            var categories = await _categoryRepository.GetByIdsAsync(task.CategoryIds);

            if (categories.Count != task.CategoryIds.Count)
            {
                throw new Exception("These categories do not exist.");
            }

   

            var existingUser = await _userRepository.GetAsync(task.UserId);
            if (existingUser == null) {
                throw new UserNotFoundException(task.UserId);
            }
            var finalTask = new TaskItem
            {
                Name = task.Name,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                UserId = existingUser.Id,
            };
            foreach (var categoryId in task.CategoryIds)
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
             
            await _taskItemRepository.DeleteAsync(taskToDelete.Id);

        }

        public async Task<List<TaskDto>> ShowAllTasksByUserIdAsync(int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }
            var tasks = await _taskItemRepository.GetAllByUserAsync(existingUser.Id);
            if (!tasks.Any())
            {
                throw new UserDoesntHaveTasksException();
            }

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                State = t.State,
                UserId = t.UserId,
                Categories = t.TasksCategories
            .Select(tc => tc.Category.Name)
            .ToList()
            }).ToList();
        }

        public async Task<TaskDto> GetTaskAsync(int id)
        {
            var task = await _taskItemRepository.GetAsync(id);

            if (task == null)
            {
                throw new TaskItemNotFoundException();
            }

            var dto = new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                State = task.State,
                UserId = task.UserId,
                Categories = task.TasksCategories
                    .Select(tc => tc.Category.Name)
                    .ToList()
            };

            return dto;
        }

        public async Task UpdateTaskAsync(UpdateTaskDto dto)
        {
            var existingTask = await _taskItemRepository.GetAsync(dto.Id);
            if (existingTask == null)
            {
                throw new TaskItemNotFoundException();
            }

            if (AreTasksEqual(existingTask, dto))
            {
                throw new UnnecessaryUpdateOperationException();
            }
            existingTask.Name = dto.Name;
            existingTask.Description = dto.Description;
            existingTask.StartDate = dto.StartDate;
            existingTask.EndDate = dto.EndDate;
            existingTask.State = dto.State;

            existingTask.TasksCategories = dto.CategoryIds.Select(categoryId => new TasksCategories
            {
                TaskId = existingTask.Id,
                CategoryId = categoryId
            }).ToList();

            await _taskItemRepository.UpdateAsync(existingTask);
        }
            
        //used in updateTaskAsync
        private static bool AreTasksEqual(TaskItem existing,UpdateTaskDto updated )
        {
            bool scalarFieldsEqual = existing.Name == updated.Name &&
                existing.Description == updated.Description &&
                existing.StartDate == updated.StartDate &&
                existing.EndDate == updated.EndDate;

            if (scalarFieldsEqual == false)
            {
                return false;
            }

            var existingCategoryIds = existing.TasksCategories.Select(tc => tc.CategoryId).ToList();
            var newCategoryIds = updated.CategoryIds;

            bool categoriesEqual = existingCategoryIds.Count == newCategoryIds.Count && !existingCategoryIds.Except(newCategoryIds).Any();

            return  categoriesEqual;
        }

        public async Task CompleteTask(int taskId)
        {
            var existingTask = await _taskItemRepository.GetAsync(taskId);
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

            var existingUser = await _userRepository.GetAsync(existingTask.UserId);
            if (existingUser == null)
            {
                throw new UserNotAssignedToTaskException();
            }

            List<TaskItem> tasksCompletedToday = await _taskItemRepository.GetCompletedTasksTodayByUser(existingUser.Id);

            //ONLY 5 TASKS WILL GIVE POINTS IN ONE DAY. SAFETY MESSURE FOR CHEATING! 
            if (tasksCompletedToday.Count <= 5)
            {
                var taskPoints = 200; // Assuming each completed task gives 200 points
                await _userService.UpdatePoints(existingUser.Id, taskPoints);
            }

            var userStats = await _userStatsService.ShowUserStatsByIdAsync(existingTask.UserId);
            userStats.TasksCompleted += 1;



            await _userStatsService.UpdateUserStatsAsync(userStats);

            await _taskItemRepository.CompleteTask(existingTask.Id);


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

        public async Task DeleteOverdueTasksMoreThanDay()
        {
            await _taskItemRepository.DeleteOverdueTaskMoreThanDay(DateTime.UtcNow);
        }

    }
}

