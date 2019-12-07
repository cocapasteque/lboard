using Newtonsoft.Json;

namespace LBoard.Models
{
    public class LeaderboardEntry
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        
        [JsonProperty("meta")]
        public string Metadata { get; set; }

        public override string ToString()
        {
            return $"[key={Key}, metadata={Metadata}]";
        }
    }
}