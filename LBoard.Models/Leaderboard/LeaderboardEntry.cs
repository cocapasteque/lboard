using Newtonsoft.Json;

namespace LBoard.Models.Leaderboard
{
    public class LeaderboardEntry
    {
        public string Key { get; set; }
        public string Metadata { get; set; }

        public override string ToString()
        {
            return $"[key={Key}, metadata={Metadata}]";
        }
    }
}