using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBoard.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LBoard.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _redis;
        private ILogger _logger;

        private IDatabase _db => _redis.GetDatabase(RedisConfig.Database);
        
        public RedisService(ILogger<RedisService> logger)
        {
            _logger = logger;

            _logger.LogInformation($"Creating Redis connection on {RedisConfig.Address}:{RedisConfig.Port}");
            _redis = ConnectionMultiplexer.Connect($"{RedisConfig.Address}:{RedisConfig.Port}");
            _logger.LogInformation($"Connected to Redis.");
        }
        
        public async Task<bool> AddToLeaderboardAsync(LeaderboardPostRequest req)
        {
            if (!RedisConfig.AllowMultiple)
            {
                if (await RemoveAsync(req.Entry))
                {
                    _logger.LogInformation($"{req.Entry} removed.");
                }
                else
                {
                    _logger.LogInformation($"{req.Entry} does not exist.");
                }
            }

            _logger.LogInformation($"Adding {req.Entry} to leaderboard.");
            var db = _db;
            return await db.SortedSetAddAsync(RedisConfig.BoardKey, JsonConvert.SerializeObject(req.Entry), req.Score);
        }
        public async Task<IEnumerable<LeaderboardEntry>> GetLeaderboardAsync(int? max = null)
        {
            _logger.LogInformation($"Asked for leaderboard.");
            var db = _db;
            var entries = await db.SortedSetRangeByRankAsync(RedisConfig.BoardKey, 0, max ?? -1L, Order.Descending);
            var stringEntries = entries.ToStringArray();
            return stringEntries.Select(x => JsonConvert.DeserializeObject<LeaderboardEntry>(x)).ToList();
        }
        private async Task<bool> RemoveAsync(LeaderboardEntry entry)
        {
            _logger.LogInformation($"Check if {entry} already exists in the leaderboard.");
            var db = _db;
            return await db.SortedSetRemoveAsync(RedisConfig.BoardKey, JsonConvert.SerializeObject(entry));
        }
    }
}