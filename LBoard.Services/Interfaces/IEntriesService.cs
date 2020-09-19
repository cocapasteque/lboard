using System.Collections.Generic;
using System.Threading.Tasks;
using LBoard.Models.Leaderboard;

namespace LBoard.Services.Interfaces
{
    public interface IEntriesService
    {
        Task<bool> AddEntryToLeaderboardAsync(string board, LeaderboardEntry entry, double score);
        Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync(string board, int? max = null);
        Task RemoveEntriesFromLeaderboardAsync(string board, string key);
    }
}