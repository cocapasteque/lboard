using Newtonsoft.Json;

namespace LBoard.Models.Leaderboard
{
    public class LeaderboardPostRequest
    {
        public LeaderboardEntry Entry { get; set; }
        public double Score { get; set; }
    }
}