using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LBoard.Context;
using LBoard.Models.Leaderboard;
using LBoard.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;

namespace LBoard.Services
{
    public class LeaderboardsService : ILeaderboardsService
    {
        private readonly LboardDbContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<IdentityUser> _users;

        private readonly ILogger<LeaderboardsService> _logger;
        private static readonly Random Random = new Random();
        
        public LeaderboardsService(ILogger<LeaderboardsService> logger, UserManager<IdentityUser> users,
            LboardDbContext context, IHttpContextAccessor http)
        {
            _logger = logger;
            _context = context;
            _http = http;
            _users = users;
        }

        public Task<IEnumerable<Leaderboard>> GetAllLeaderboardsAsync()
        {
            return Task.Run(() =>
            {
                var id = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return _context.Leaderboards.Where(x => x.OwnerId.Equals(id)).AsEnumerable();
            });
        }

        public Task<Leaderboard> AddLeaderboardAsync(Leaderboard leaderboard)
        {
            return Task.Run(async () =>
            {
                var id = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _users.FindByIdAsync(id);
                
                leaderboard.Key = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10).Select(s => s[Random.Next(s.Length)]).ToArray());
                leaderboard.Owner = user;
                var entity = await _context.Leaderboards.AddAsync(leaderboard);
                await _context.SaveChangesAsync();
                return entity.Entity;
            });
        }

        public Task<Leaderboard> UpdateLeaderboardAsync(Leaderboard leaderboard)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveLeaderboardAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveLeaderboardAsync(Leaderboard leaderboard)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveLeaderboardAsync(Func<Leaderboard, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Leaderboard> GetLeaderboardAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Leaderboard> GetLeaderboardAsync(Func<Leaderboard, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}