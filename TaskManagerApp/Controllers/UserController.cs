using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using TaskManagerApp.DTOS;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // =====================================================
        // GET USER BY ID
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user =
                await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // =====================================================
        // REGISTER
        // =====================================================

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserDto registerUserDto)
        {
            await _userService.RegisterUser(
                registerUserDto);

            return Ok(new
            {
                Message =
                    "Registration successful. Please verify your email."
            });
        }


        // =====================================================
        // LOGIN
        // =====================================================

        [HttpPost("login")]
        public async Task<IActionResult> Login(
    [FromBody] LoginUserDto loginUserDto)
        {
            var user =
                await _userService.LogInUser(
                    loginUserDto);

            if (user == null)
            {
                return Unauthorized(new
                {
                    error = "Invalid email or password."
                });
            }


            // =================================================
            // CHECK EMAIL VERIFICATION
            // =================================================

            var isEmailVerified =
                await _userService.IsEmailVerified(
                    loginUserDto.Email);

            if (!isEmailVerified)
            {
                // Изпращаме нов 6-цифрен код
                await _userService.SendVerificationCode(
                    loginUserDto.Email);

                // НЕ създаваме authentication cookie още
                return Ok(new
                {
                    requiresEmailVerification = true,
                    email = loginUserDto.Email,
                    message = "Verification code sent to your email."
                });
            }


            // =================================================
            // CLAIMS
            // =================================================

            var claims = new List<Claim>
    {
        new Claim(
            ClaimTypes.NameIdentifier,
            user.Id.ToString()),

        new Claim(
            ClaimTypes.Name,
            user.Username),

        new Claim(
            ClaimTypes.Email,
            user.Email)
    };


            // =================================================
            // IDENTITY
            // =================================================

            var identity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                        .AuthenticationScheme);


            // =================================================
            // PRINCIPAL
            // =================================================

            var principal =
                new ClaimsPrincipal(identity);


            // =================================================
            // COOKIE SETTINGS
            // =================================================

            var properties =
                new AuthenticationProperties
                {
                    IsPersistent = true,

                    ExpiresUtc =
                        DateTimeOffset.UtcNow
                            .AddDays(30),

                    AllowRefresh = true
                };


            // =================================================
            // CREATE COOKIE
            // =================================================

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme,

                principal,

                properties);


            return Ok(new
            {
                requiresEmailVerification = false,
                user = user,
                message = "Login successful."
            });
        }


        // =====================================================
        // CURRENT USER
        // =====================================================

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // =====================================================
        // LOGOUT
        // =====================================================

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme);

            return Ok(new
            {
                Message = "Logout successful."
            });
        }


        // =====================================================
        // VERIFY EMAIL
        // =====================================================

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromBody] VerifyEmailDto verifyEmailDto)
        {
            await _userService.VerifyEmail(
                verifyEmailDto);

            return Ok(new
            {
                Message =
                    "Email verified successfully."
            });
        }

        [HttpGet("verify-email-and-login")]
        public async Task<IActionResult> VerifyEmailAndLogin(
    [FromQuery] string email,
    [FromQuery] string code)
        {
            try
            {
                var verifyEmailDto = new VerifyEmailDto
                {
                    Email = email,
                    Code = code
                };

                await _userService.VerifyEmail(verifyEmailDto);

                var user = await _userService.GetUserByEmail(email);

                if (user == null)
                {
                    return Unauthorized();
                }

                var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.Username),

            new Claim(
                ClaimTypes.Email,
                user.Email)
        };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    properties);

                return Redirect("https://localhost:7254/dashboard");
            }
            catch (Exception)
            {
                return Redirect(
                    $"https://localhost:7254/verify-code" +
                    $"?email={Uri.EscapeDataString(email)}" +
                    $"&error=invalid");
            }
        }

        // =====================================================
        // UPDATE POINTS
        // =====================================================

        [Authorize]
        [HttpPut("{userId:int}/points")]
        public async Task<IActionResult> UpdatePoints(
            int userId,
            [FromQuery] int points)
        {
            await _userService.UpdatePoints(
                userId,
                points);

            return NoContent();
        }


        // =====================================================
        // DELETE ACCOUNT
        // =====================================================

        [Authorize]
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DeleteAccount(
            int userId)
        {
            await _userService.DeleteAccount(
                userId);

            return NoContent();
        }
    }
}