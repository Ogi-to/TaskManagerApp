using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksParticipantsController : ControllerBase
    {
        private readonly ITasksParticipantsService _tasksParticipantsService;

        public TasksParticipantsController(ITasksParticipantsService tasksParticipantsService)
        {
            _tasksParticipantsService = tasksParticipantsService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<TasksParticipantsDto>>> GetByUserId(int userId)
        {
            var participants = await _tasksParticipantsService.GetByUserId(userId);
            return Ok(participants);
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<List<TasksParticipantsDto>>> GetByTaskId(int taskId)
        {
            var participants = await _tasksParticipantsService.GetByTaskId(taskId);
            return Ok(participants);
        }

        [HttpGet("task/{taskId}/user/{userId}")]
        public async Task<ActionResult<TasksParticipantsDto>> GetByBoth(int taskId, int userId)
        {
            var participant = await _tasksParticipantsService.GetByBoth(taskId, userId);
            return Ok(participant);
        }

        [HttpDelete("task/{taskId}/user/{userId}")]
        public async Task<IActionResult> DeleteByBoth(int taskId, int userId)
        {
            await _tasksParticipantsService.DeleteByBoth(taskId, userId);
            return NoContent();
        }

        [HttpPost("invite")]
        public async Task<IActionResult> InviteFriendsToTask(
            [FromQuery] int ownerId,
            [FromQuery] int friendId,
            [FromQuery] int taskId)
        {
            await _tasksParticipantsService.InviteFriendsToTask(ownerId, friendId, taskId);
            return Ok();
        }

        [HttpPut("answer")]
        public async Task<IActionResult> AnswerInviteForTask(
            [FromQuery] int friendId,
            [FromQuery] int taskId,
            [FromQuery] Status status)
        {
            await _tasksParticipantsService.AnswerInviteForTask(friendId, taskId, status);
            return NoContent();
        }
    }
}