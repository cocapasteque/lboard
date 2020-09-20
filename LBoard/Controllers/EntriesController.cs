using System.Threading.Tasks;
using LBoard.Models.Leaderboard;
using LBoard.Services;
using LBoard.Services.Extensions;
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
            var boardId = $"{HttpContext.GetUserId()}_{board}";
            var entries = await _redis.GetEntriesAsync(boardId);
            return Ok(entries);
        }
        
        [HttpPost("{board}")]
        public async Task<IActionResult> PostEntry([FromBody] LeaderboardPostRequest req, string board)
        {
            var boardId = $"{HttpContext.GetUserId()}_{board}";
            var ok = await _redis.AddEntryToLeaderboardAsync(boardId, req.Entry, req.Score);
            if (ok) return Ok(req);
            return StatusCode(500);
        }

        [HttpDelete("{board}/{key}")]
        public async Task<IActionResult> RemoveEntry(string key, string board)
        {
            var boardId = $"{HttpContext.GetUserId()}_{board}";
            await _redis.RemoveEntriesFromLeaderboardAsync(boardId, key);
            return StatusCode(200);
        }
    }
}