using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.DTOS;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendController(
            IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }


        // ============================================
        // SEND FRIEND REQUEST
        // ============================================

        [HttpPost("request")]
        public async Task<IActionResult> SendFriendRequest(
            [FromBody] SendFriendRequestDto dto)
        {
            try
            {
                var friendship =
                    await _friendshipService
                        .SendFriendRequestAsync(
                            dto.SenderId,
                            dto.ReceiverId);

                return Ok(friendship);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }


        // ============================================
        // GET FRIENDS
        // ============================================

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFriends(
            int userId)
        {
            try
            {
                var friends =
                    await _friendshipService
                        .GetFriendsAsync(userId);

                return Ok(friends);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }


        // ============================================
        // GET PENDING REQUESTS
        // ============================================

        [HttpGet("requests/{userId}")]
        public async Task<IActionResult> GetPendingRequests(
            int userId)
        {
            try
            {
                var requests =
                    await _friendshipService
                        .GetPendingRequestsAsync(userId);

                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }


        // ============================================
        // ACCEPT FRIEND REQUEST
        // ============================================

        [HttpPut("accept/{friendshipId}")]
        public async Task<IActionResult> AcceptFriendRequest(
            int friendshipId,
            [FromQuery] int userId)
        {
            try
            {
                await _friendshipService
                    .AcceptFriendRequestAsync(
                        userId,
                        friendshipId);

                return Ok(new
                {
                    message = "Friend request accepted."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }


        // ============================================
        // REJECT FRIEND REQUEST
        // ============================================

        [HttpPut("reject/{friendshipId}")]
        public async Task<IActionResult> RejectFriendRequest(
            int friendshipId,
            [FromQuery] int userId)
        {
            try
            {
                await _friendshipService
                    .RejectFriendRequestAsync(
                        userId,
                        friendshipId);

                return Ok(new
                {
                    message = "Friend request rejected."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }


        // ============================================
        // REMOVE FRIEND
        // ============================================

        [HttpDelete("{friendshipId}")]
        public async Task<IActionResult> RemoveFriend(
            int friendshipId,
            [FromQuery] int userId)
        {
            try
            {
                await _friendshipService
                    .RemoveFriendAsync(
                        userId,
                        friendshipId);

                return Ok(new
                {
                    message = "Friend removed successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }
    }
}