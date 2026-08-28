using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.DTOS;
using TaskManagerApp.Services;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AccountController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginUserDto loginUserDto)
        {
            var result = await _jwtService.Authenticate(loginUserDto);
            if (result == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(result);
        }
    }
}
