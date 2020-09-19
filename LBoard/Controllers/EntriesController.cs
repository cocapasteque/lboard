using System.Threading.Tasks;
using LBoard.Models.Leaderboard;
using LBoard.Services;
using LBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EntriesController : ControllerBase
    {
        private readonly IEntriesService _redis;
        
        public EntriesController(IEntriesService redis)
        {
            _redis = redis;
        }

        [HttpGet("{board}")]
        public async Task<IActionResult> GetLeaderboard(string board)
        {
            var entries = await _redis.GetEntriesAsync(board);
            return Ok(entries);
        }
        
        [HttpPost("{board}")]
        public async Task<IActionResult> PostEntry([FromBody] LeaderboardPostRequest req, string board)
        {
            var ok = await _redis.AddEntryToLeaderboardAsync(board, req.Entry, req.Score);
            if (ok) return Ok(req);
            return StatusCode(500);
        }

        [HttpDelete("{board}/{key}")]
        public async Task<IActionResult> RemoveEntry(string key, string board)
        {
            await _redis.RemoveEntriesFromLeaderboardAsync(board, key);
            return StatusCode(200);
        }
    }
}