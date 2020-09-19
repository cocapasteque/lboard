using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace LBoard.Models.Leaderboard
{
    public class Leaderboard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string OwnerId { get; set; }
        
        [JsonIgnore]
        public IdentityUser Owner { get; set; }

        public override string ToString()
        {
            return $"[Key={Key}, Name={Name}, Description={Description}, Owner={Owner}]";
        }
    }
}