using System.Collections.Generic;
using System.Threading.Tasks;
using LBoard.Models;

namespace LBoard.Services
{
    public interface ILeaderboardService
    {
        Task<bool> AddToLeaderboardAsync(LeaderboardEntry entry, double score);
        Task<IEnumerable<LeaderboardEntry>> GetLeaderboardAsync(int? max = null);
    }
}