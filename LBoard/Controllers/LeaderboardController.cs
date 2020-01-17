using System.Threading.Tasks;
using LBoard.Models;
using LBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace LBoard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _redis;
        
        public LeaderboardController(ILeaderboardService redis)
        {
            _redis = redis;
        }

        [HttpGet]
        public async Task<ActionResult> GetLeaderboard()
        {
            var entries = await _redis.GetLeaderboardAsync();
            return Ok(entries);
        }
        
        [HttpPost]
        public async Task<ActionResult> PostEntry([FromBody] LeaderboardPostRequest req)
        {
            var ok = await _redis.AddToLeaderboardAsync(req.Entry, req.Score);
            if (ok) return Ok(req);
            return StatusCode(500);
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult> RemoveEntry(string key)
        {
            return StatusCode(500);
        }
    }
}