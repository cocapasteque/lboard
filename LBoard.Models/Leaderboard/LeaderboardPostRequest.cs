using Newtonsoft.Json;

namespace LBoard.Models.Leaderboard
{
    public class LeaderboardPostRequest
    {
        [JsonProperty("entry")]
        public LeaderboardEntry Entry { get; set; }
        
        [JsonProperty("score")]
        public double Score { get; set; }
    }
}