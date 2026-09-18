using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Services;
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            await _userService.RegisterUser(dto);
            return Ok("User registered successfully.");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            await _userService.LogInUser(loginUserDto, ipAddress);

            return Ok(new
            {
                Message = "Verification code has been sent to your email."
            });
        }

        [HttpPost("verify-login")]
        public async Task<ActionResult<UserDto>> VerifyLogin([FromBody] VerifyEmailDto verifyLoginDto)
        {
            UserDto user = await _userService.UseTheSendCodeForLogin(verifyLoginDto);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await _userService.DeleteAccount(id);
            return NoContent();
        }

        [HttpPut("{id}/points")]
        public async Task<IActionResult> UpdatePoints(int id, [FromQuery] int points)
        {
            await _userService.UpdatePoints(id, points);
            return NoContent();
        }

        [HttpPost("{initiatorId}/friend-request/{relatedUserId}")]
        public async Task<IActionResult> SendFriendRequest(int initiatorId, int relatedUserId)
        {
            await _userService.SendFriendRequest(initiatorId, relatedUserId);
            return Ok();
        }

        [HttpGet("{id}/friend-requests")]
        public async Task<ActionResult<List<UsersRelationsDto>>> GetPendingRequests(int id)
        {
            return Ok(await _userService.GetUnansweredRelationReceivedByUserIdAsync(id));
        }

        [HttpPut("{relatedUserId}/friend-request/{initiatorId}")]
        public async Task<IActionResult> AnswerFriendRequest(
            int relatedUserId,
            int initiatorId,
            [FromQuery] RelationStatus relationStatus)
        {
            await _userService.AnswerToSentRequest(
                relatedUserId,
                initiatorId,
                relationStatus);

            return NoContent();
        }

        [HttpPut("{initiatorId}/relation/{relatedUserId}")]
        public async Task<IActionResult> UpdateRelation(
            int initiatorId,
            int relatedUserId,
            [FromQuery] RelationType relationType)
        {
            await _userService.UpdateUserRelation(
                initiatorId,
                relatedUserId,
                relationType);

            return NoContent();
        }

        [HttpDelete("relations/old")]
        public async Task<IActionResult> DeleteOldRelations()
        {
            await _userService.DeleteUserRelationByMoreThanAMonth();
            return NoContent();
        }

        [HttpPut("reminder-settings")]
        public async Task<IActionResult> UpdateReminderSettings(
            [FromBody] ReminderSettingsDto dto)
        {
            await _userService.UpdateReminderSettings(dto);
            return NoContent();
        }


        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromBody] VerifyEmailDto dto)
        {
            await _userService.VerifyEmail(dto);
            return Ok("Email verified.");
        }

        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerificationCode(
            [FromQuery] string email)
        {
            await _userService.ReSendVerificationCode(email);
            return Ok();
        }

        [HttpPost("send-task-reminders")]
        public async Task<IActionResult> SendTaskReminders()
        {
            await _userService.SendReminderForTasksEmail();
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

