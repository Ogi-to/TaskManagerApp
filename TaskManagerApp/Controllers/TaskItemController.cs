using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : Controller
    {
        private ITaskItemRepository _taskItemRepository;

        public TaskItemController(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] TaskItem taskItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var addedTaskItem = _taskItemRepository.AddTaskAsync(taskItem);
            if (addedTaskItem == null) {
                return BadRequest("Failed to add task item.");
            }
            return CreatedAtAction(nameof(Get), new { id = addedTaskItem.Id }, addedTaskItem);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var taskItem = _taskItemRepository.GetAsync(id).Result;
            if (taskItem == null)
            {
                return NotFound();
            }
            return Ok(taskItem);
        }


    }
}
