using System.Collections.Generic;
using System.Threading.Tasks;
using LBoard.Models;
using LBoard.Models.Leaderboard;

namespace LBoard.Services
{
    public interface ILeaderboardService
    {
        Task<bool> AddToLeaderboardAsync(string board, LeaderboardEntry entry, double score);
        Task<IEnumerable<LeaderboardEntry>> GetLeaderboardAsync(string board, int? max = null);
        Task RemoveFromLeaderboardAsync(string board, string key);
    }
}