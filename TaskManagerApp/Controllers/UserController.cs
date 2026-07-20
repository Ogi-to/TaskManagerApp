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

        // GET: api/User/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            await _userService.RegisterUser(registerUserDto);
            return Ok(new
            {
                Message = "Registration successful. Please verify your email."
            });
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginUserDto loginUserDto)
        {
            var user = await _userService.LogInUser(loginUserDto);
            return Ok(user);
        }

        // POST: api/User/verify-email
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
        {
            await _userService.VerifyEmail(verifyEmailDto);
            return Ok(new { Message = "Email verified successfully." });
        }

        // PUT: api/User/5/points
        [HttpPut("{userId:int}/points")]
        public async Task<IActionResult> UpdatePoints(
            int userId,
            [FromQuery] int points)
        {
            await _userService.UpdatePoints(userId, points);
            return NoContent();
        }

        [HttpPut("{userId:int}/reminder-settings")]
        public async Task<IActionResult> UpdateReminderSettings(
            int userId,
            [FromBody] ReminderSettingsDto reminderSettingsDto)
        {
            await _userService.UpdateReminderSettings(reminderSettingsDto);
            return NoContent();
        }

        [HttpPost("/Resend verification code")]
        public async Task<IActionResult> ReSendVerificationCode(string email)
        {
            await _userService.ReSendVerificationCode(email);
            return NoContent();
        }


        // DELETE: api/User/5
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DeleteAccount(int userId)
        {
            await _userService.DeleteAccount(userId);
            return NoContent();
        }
    

}
}
