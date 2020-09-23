using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using LBoard.Context;
using LBoard.Models.Leaderboard;
using LBoard.Services.Extensions;
using LBoard.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
                var id = _http.HttpContext.GetUserId();
                return _context.Leaderboards.Where(x => x.OwnerId.Equals(id)).AsEnumerable();
            });
        }

        public Task<Leaderboard> AddLeaderboardAsync(Leaderboard leaderboard)
        {
            return Task.Run(async () =>
            {
                var id = _http.HttpContext.GetUserId();
                var user = await _users.FindByIdAsync(id);

                leaderboard.Key = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());
                leaderboard.Owner = user;
                var entity = await _context.Leaderboards.AddAsync(leaderboard);
                await _context.SaveChangesAsync();
                return entity.Entity;
            });
        }

        public Task<Leaderboard> UpdateLeaderboardAsync(Leaderboard leaderboard)
        {
            return Task.Run(async () =>
            {
                var id = _http.HttpContext.GetUserId();
                var entry =
                    await _context.Leaderboards.FirstOrDefaultAsync(x =>
                        x.OwnerId.Equals(id) && x.Id.Equals(leaderboard.Id));

                if (entry == null) return null;

                entry.Description = leaderboard.Description;
                entry.Name = leaderboard.Name;
                
                var entity = _context.Leaderboards.Update(entry);
                await _context.SaveChangesAsync();
                return entity.Entity;
            });
        }

        public Task<bool> RemoveLeaderboardAsync(string id)
        {
            return Task.Run(async () =>
            {
                var userId = _http.HttpContext.GetUserId();
                var entry = await _context.Leaderboards.FirstOrDefaultAsync(
                    x => x.OwnerId.Equals(userId) && x.Id.Equals(id));
                if (entry == null) return false;

                var result = _context.Leaderboards.Remove(entry);
                await _context.SaveChangesAsync();

                return result != null;
            });
        }

        public Task<bool> RemoveLeaderboardAsync(Leaderboard leaderboard)
        {
            return Task.Run(async () =>
            {
                var result = _context.Leaderboards.Remove(leaderboard);
                await _context.SaveChangesAsync();

                return result != null;
            });
        }

        public Task<bool> RemoveLeaderboardAsync(Expression<Func<Leaderboard, bool>> predicate)
        {
            return Task.Run(async () =>
            {
                var userId = _http.HttpContext.GetUserId();
                var entry = await _context.Leaderboards.Where(
                    x => x.OwnerId.Equals(userId)).FirstOrDefaultAsync(predicate);
                
                if (entry == null) return false;

                var result = _context.Leaderboards.Remove(entry);
                await _context.SaveChangesAsync();

                return result != null;
            });
        }

        public Task<Leaderboard> GetLeaderboardAsync(string id)
        {
            return Task.Run(async () =>
            {
                var userId = _http.HttpContext.GetUserId();
                var leaderboard =
                    await _context.Leaderboards.FirstOrDefaultAsync(x => x.OwnerId.Equals(userId) && x.Id.Equals(id));
                return leaderboard;
            });
        }

        public Task<Leaderboard> GetLeaderboardAsync(Expression<Func<Leaderboard, bool>> predicate)
        {
            return Task.Run(async () =>
            {
                var userId = _http.HttpContext.GetUserId();
                if (predicate == null) return null;

                var leaderboards = _context.Leaderboards.Where(x => x.OwnerId.Equals(userId));
                if (!leaderboards.Any()) return null;

                var leaderboard = await leaderboards.FirstOrDefaultAsync(predicate);
                return leaderboard;
            });
        }
    }
}