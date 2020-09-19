using System;

namespace LBoard.Models.Security.ApiKey
{
    public class LBoardApiKey
    {
        public LBoardApiKey(string id, string owner, string key, DateTime created)
        {
            Id = id;
            Owner = owner;
            Key = key;
            Created = created;
        }
        
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Key { get; set; }
        public DateTime Created { get; set; }
    }
}