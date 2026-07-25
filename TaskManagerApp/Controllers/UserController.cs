using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace TaskManagerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            return Ok(await _userService.GetUserById(id));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            await _userService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto dto)
        {
            return Ok(await _userService.LogInUser(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await _userService.DeleteAccount(id);
            return Ok();
        }

        [HttpPut("{userId}/points/{points}")]
        public async Task<IActionResult> UpdatePoints(int userId, int points)
        {
            await _userService.UpdatePoints(userId, points);
            return Ok();
        }

        [HttpPost("friend-request")]
        public async Task<IActionResult> SendFriendRequest(int initiatorId, int relatedUserId)
        {
            await _userService.SendFriendRequest(initiatorId, relatedUserId);
            return Ok();
        }

        [HttpGet("{relatedUserId}/pending-requests")]
        public async Task<ActionResult<List<UsersRelations>>> GetPendingRequests(int relatedUserId)
        {
            return Ok(await _userService.GetUnansweredRelationReceivedByUserIdAsync(relatedUserId));
        }

        [HttpPost("answer-request")]
        public async Task<IActionResult> AnswerRequest(
            int relatedUserId,
            int userInitiatorId,
            RelationStatus relationStatus)
        {
            await _userService.AnswerToSentRequest(
                relatedUserId,
                userInitiatorId,
                relationStatus);

            return Ok();
        }

        [HttpPut("reminder-settings")]
        public async Task<IActionResult> UpdateReminderSettings(ReminderSettingsDto dto)
        {
            await _userService.UpdateReminderSettings(dto);
            return Ok();
        }

        [HttpPost("send-task-reminders")]
        public async Task<IActionResult> SendTaskReminders()
        {
            await _userService.SendReminderForTasksEmail();
            return Ok();
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto dto)
        {
            await _userService.VerifyEmail(dto);
            return Ok();
        }

        [HttpPost("resend-code")]
        public async Task<IActionResult> ResendVerificationCode([FromBody] string email)
        {
            await _userService.ReSendVerificationCode(email);
            return Ok();
        }

        [HttpPost("send-streak-reminders")]
        public async Task<IActionResult> SendStreakReminders()
        {
            await _userService.SendReminderForStreakEmail();
            return Ok();
        }
    }


}

