using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBoard.Models;
using LBoard.Models.Config;
using LBoard.Models.Leaderboard;
using LBoard.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LBoard.Services
{
    public class RedisService : IEntriesService
    {
        private readonly ConnectionMultiplexer _redis;
        private IDatabase _db => _redis.GetDatabase(RedisConfig.Database);

        private readonly ILogger _logger;

        public RedisService(ILogger<RedisService> logger)
        {
            _logger = logger;

            _logger.LogInformation($"Creating Redis connection on {RedisConfig.Address}:{RedisConfig.Port}");
            _redis = ConnectionMultiplexer.Connect($"{RedisConfig.Address}:{RedisConfig.Port},password={RedisConfig.Password}");
            _logger.LogInformation($"Connected to Redis.");
        }

        public async Task<bool> AddEntryToLeaderboardAsync(string board, LeaderboardEntry entry, double score)
        {
            if (await RemoveAsync(board, entry))
            {
                _logger.LogInformation($"{entry} removed in {board}.");
            }
            else
            {
                _logger.LogInformation($"{entry} does not exist in {board}.");
            }


            _logger.LogInformation($"Adding {entry} to leaderboard {board}.");
            var db = _db;
            return await db.SortedSetAddAsync(board, JsonConvert.SerializeObject(entry), score);
        }

        public async Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync(string board, int? max = null)
        {
            _logger.LogInformation($"Asked for leaderboard {board}.");
            var db = _db;
            var entries = await db.SortedSetRangeByRankAsync(board, 0, max ?? -1L, Order.Descending);
            var stringEntries = entries.ToStringArray();
            return stringEntries.Select(JsonConvert.DeserializeObject<LeaderboardEntry>).ToList();
        }

        private async Task<bool> RemoveAsync(string board, LeaderboardEntry entry)
        {
            _logger.LogInformation($"Check if {entry} already exists in the leaderboard {board}.");
            var db = _db;
            return await db.SortedSetRemoveAsync(board, JsonConvert.SerializeObject(entry));
        }

        public async Task RemoveEntriesFromLeaderboardAsync(string board, string key)
        {
            _logger.LogInformation($"Removing all entries for {key} in {board}");
            var db = _db;
            var entries = await GetEntriesAsync(board);
            var toDelete = entries.Where(x => x.Key == key).Select(JsonConvert.SerializeObject);
            _logger.LogInformation($"Found {toDelete.Count()} entries to delete");

            foreach (var item in toDelete)
            {
                db.SortedSetRemoveAsync(board, item);
            }
            
            _logger.LogInformation("deleted");
        }
    }
}