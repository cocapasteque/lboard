using System.Threading.Tasks;
using LBoard.Models.Leaderboard;
using LBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaderboardsController : ControllerBase
    {
        private readonly ILeaderboardsService _leaderboards;
        private readonly ILogger<LeaderboardsController> _logger;
        
        public LeaderboardsController(ILogger<LeaderboardsController> logger, ILeaderboardsService leaderboards)
        {
            _logger = logger;
            _leaderboards = leaderboards;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaderboards()
        {
            var boards = await _leaderboards.GetAllLeaderboardsAsync();
            return Ok(boards);
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetLeaderboard(string key)
        {
            var board = await _leaderboards.GetLeaderboardAsync(x => x.Key.Equals(key));
            return Ok(board);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> RemoveLeaderboard(string key)
        {
            var board = await _leaderboards.RemoveLeaderboardAsync(x => x.Key.Equals(key));
            return Ok($"{(board ? "Success" : "Failure")}");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLeaderboard([FromBody] Leaderboard leaderboard)
        {
            var board = await _leaderboards.UpdateLeaderboardAsync(leaderboard);
            return Ok(board);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddLeaderboard([FromBody] Leaderboard leaderboard)
        {
            var board = await _leaderboards.AddLeaderboardAsync(leaderboard);
            return Ok(board);
        }
    }
}