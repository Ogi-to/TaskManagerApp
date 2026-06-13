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
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] User user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_userRepository.Get(user) != null)
            {
                return Conflict("User with the same username or email already exists.");
            }


            var createdUser = _userRepository.CreateAccount(user);
            if (createdUser == null)
            {
                return StatusCode(500, "An error occurred while creating the account.");
            }

            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }


        [HttpGet("{id}")]
        public IActionResult Get(User item)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item == null)
            {
                return NotFound();
            }
            var user = _userRepository.Get(item);

            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public IActionResult GetByEmail(string email)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (email == null)
            {
                return NotFound();
            }
            var user = _userRepository.GetByEmail(email);

            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public IActionResult GetByUsername(string username)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (username == null)
            {
                return NotFound();
            }
            var user = _userRepository.GetByUsername(username);

            return Ok(user);
        }

        [HttpGet("userCode/{userCode}")]
        public IActionResult GetByUserCode(string userCode)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userCode == null)
            {
                return NotFound();
            }
            var user = _userRepository.GetByUserCode(userCode);

            return Ok(user);
        }

        [HttpGet("getAll-users")]
        public IActionResult GetAllUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = _userRepository.GetAllUsers();

            return Ok(users);


        }

        [HttpPut("update-account-info")]
        public IActionResult UpdateAccountInfo([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_userRepository.Get(user) == null)
            {
                return NotFound();
            }
            _userRepository.UpdateAccountInfo(user);
            return NoContent();
        }

        [HttpPut("update-user-info")]
        public IActionResult UpdateUserInfo([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_userRepository.Get(user) == null)
            {
                return NotFound();
            }
            _userRepository.UpdateUserInfo(user);
            return NoContent();

        }


        [HttpPut("respond")]
        public IActionResult RespondToRequest([FromBody] UsersRelations relation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exist = _userRepository.GetRelation(
                relation.InitiatorId,
                relation.RelatedUserId
            );

            if (exist == null)
                return NotFound();

            _userRepository.RespondToRequest(relation);

            return NoContent();
        }

        [HttpPost("send-request")]
        public IActionResult SendRequest([FromBody] UsersRelations relation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = _userRepository.GetRelation(
                relation.InitiatorId,
                relation.RelatedUserId
            );

            if (exists != null)
            {
                return Conflict("Request already exists");
            }
            
            _userRepository.SendRequest(relation);

            return Ok(relation);
        }

        [HttpGet("friends/{userId}")]
        public IActionResult GetFriendsList(User item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepository.Get(item);

            if (user == null)
            {
                return NotFound();
            }

            var friends = _userRepository.GetFriendsList(user);

            return Ok(friends);
        }
    }
}
