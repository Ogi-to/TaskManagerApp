using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly TaskItemRepository _taskItemRepository;
        private readonly UserRepository _userRepository;
        public TaskItemService(TaskItemRepository repository, UserRepository userRepository)
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


            await _taskItemRepository.AssignToUserAsync(existingTask, existingUser);

        }

        public Task DeleteTaskAsync(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskItem>> ShowAllTasksByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskItem> ShowTaskAsync(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(TaskItem task)
        {
            throw new NotImplementedException();
        }
    }
}
