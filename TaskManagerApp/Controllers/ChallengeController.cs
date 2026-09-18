using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.DTOS;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallengeController : ControllerBase
    {
        private readonly IChallengeService _challengeService;

        public ChallengeController(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<ChallengeDto> challenges = await _challengeService.ShowAllAsync();
            if (challenges == null || !challenges.Any())
            {
                return NotFound("No challenges found.");
            }
            if (challenges.Any())
            {
                return Ok(await _challengeService.ShowAllAsync());
            }
            else
            {
                return BadRequest();
            }
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _challengeService.ShowAsync(id));
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            return Ok(await _challengeService.ShowAllByCategoryAsync(categoryId));
        }

        [HttpGet("level/{level}")]
        public async Task<IActionResult> GetByLevel(int level)
        {
            return Ok(await _challengeService.ShowAllByLevelAsync(level));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            return Ok(await _challengeService.ShowAllByUserAsync(userId));
        }

        [HttpGet("user/{userId}/completed")]
        public async Task<IActionResult> GetCompletedByUser(int userId)
        {
            return Ok(await _challengeService.ShowAllCompletedByUserAsync(userId));
        }

        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActive()
        {
            return Ok(await _challengeService.GetAllActive());
        }

        [HttpPost("{challengeId}/join/{userId}")]
        public async Task<IActionResult> JoinChallenge(int challengeId, int userId)
        {
            await _challengeService.JoinChallengeAsync(challengeId, userId);

            return Ok("User joined the challenge.");
        }

        [HttpPut("{challengeId}/complete/{userId}")]
        public async Task<IActionResult> CompleteChallenge(int challengeId, int userId)
        {
            await _challengeService.CompleteChallengeAsync(challengeId, userId);

            return Ok("Challenge completed.");
        }

        [HttpPost("choose-random")]
        public async Task<IActionResult> ChooseRandomChallenges()
        {
            await _challengeService.ChooseRandomChallenges();

            return Ok("Random challenges selected.");
        }

        [HttpDelete("remove-expired")]
        public async Task<IActionResult> RemoveExpiredChallenges()
        {
            await _challengeService.RemoveChallengesActivity();

            return Ok("Expired challenges removed.");
        }
    }
}
