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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                await _userService.RegisterUser(dto);
                return Ok(new { message = "User registered successfully" });
            }
            catch (UserNameAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserEmailAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            try
            {
                var user = await _userService.LogInUser(dto);
                return Ok(user);
            }
            catch (EmailorPasswordNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EmailNotVerifiedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string email, [FromQuery] string code)
        {
            try
            {
                var result = await _userService.VerifyEmail(email, code);
                return Ok(new { success = result });
            }
            catch (InvalidVerificationCodeException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
