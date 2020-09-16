using System.Threading.Tasks;
using LBoard.Models;
using LBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace LBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _redis;
        
        public LeaderboardController(ILeaderboardService redis)
        {
            _redis = redis;
        }

        [HttpGet("{board}")]
        public async Task<ActionResult> GetLeaderboard(string board)
        {
            var entries = await _redis.GetLeaderboardAsync(board);
            return Ok(entries);
        }
        
        [HttpPost("{board}")]
        public async Task<ActionResult> PostEntry([FromBody] LeaderboardPostRequest req, string board)
        {
            var ok = await _redis.AddToLeaderboardAsync(board, req.Entry, req.Score);
            if (ok) return Ok(req);
            return StatusCode(500);
        }

        [HttpDelete("{board}/{key}")]
        public async Task<ActionResult> RemoveEntry(string key, string board)
        {
            await _redis.RemoveFromLeaderboardAsync(board, key);
            return StatusCode(200);
        }
    }
}