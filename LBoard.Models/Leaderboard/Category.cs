using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace LBoard.Models.Leaderboard
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        
        public List<Leaderboard> Leaderboards { get; set; }
        
        public string OwnerId { get; set; }
        [JsonIgnore]
        public IdentityUser Owner { get; set; }
    }
}