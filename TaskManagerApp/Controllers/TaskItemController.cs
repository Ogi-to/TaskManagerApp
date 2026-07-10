using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace TaskManagerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;

        public TaskItemController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] AddTaskRequest request)
        {
            await _taskItemService.AddTaskAsync(
                request.Task,
                request.User.Id,
                request.CategoryIds);

            return Ok("Task added successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskItemService.DeleteTaskAsync(id);

            return Ok("Task deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskItemService.ShowTaskAsync(id);

            return Ok(task);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserTasks(int userId)
        {
            var tasks = await _taskItemService.ShowAllTasksByUserIdAsync(userId);

            return Ok(tasks);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskItem task)
        {
            await _taskItemService.UpdateTaskAsync(task);

            return Ok("Task updated successfully.");
        }

        [HttpPut("complete")]
        public async Task<IActionResult> CompleteTask([FromBody] TaskItem task)
        {
            await _taskItemService.CompleteTask(task);

            return Ok("Task completed.");
        }

        [HttpPut("mark-overdue")]
        public async Task<IActionResult> MarkOverdueTasks()
        {
            await _taskItemService.MarkOverdueTasksAsync();

            return Ok("Overdue tasks updated.");
        }

        [HttpDelete("delete-old-overdue")]
        public async Task<IActionResult> DeleteOldOverdueTasks()
        {
            await _taskItemService.DeleteOverdueTasksMoreThanDay();

            return Ok("Old overdue tasks deleted.");
        }
    }

    public class AddTaskRequest
    {
        public TaskItem Task { get; set; } = new();
        public User User { get; set; } = new();
        public List<int> CategoryIds { get; set; } = new();
    }

}

