﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBoard.Models.Leaderboard;

namespace LBoard.Services.Interfaces
{
    public interface ILeaderboardsService
    {
        Task<IEnumerable<Leaderboard>> GetAllLeaderboardsAsync();
        
        Task<Leaderboard> AddLeaderboardAsync(Leaderboard leaderboard);

        Task<Leaderboard> UpdateLeaderboardAsync(Leaderboard leaderboard);

        Task<bool> RemoveLeaderboardAsync(string id);
        Task<bool> RemoveLeaderboardAsync(Leaderboard leaderboard);
        Task<bool> RemoveLeaderboardAsync(Func<Leaderboard, bool> predicate);

        Task<Leaderboard> GetLeaderboardAsync(string id);
        Task<Leaderboard> GetLeaderboardAsync(Func<Leaderboard, bool> predicate);
    }
}