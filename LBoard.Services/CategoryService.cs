using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LBoard.Context;
using LBoard.Models.Leaderboard;
using LBoard.Services.Extensions;
using LBoard.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LBoard.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly LboardDbContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<IdentityUser> _users;

        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ILogger<CategoryService> logger, UserManager<IdentityUser> users,
            LboardDbContext context, IHttpContextAccessor http)
        {
            _logger = logger;
            _context = context;
            _http = http;
            _users = users;
        }

        public Task<Category> AddCategoryAsync(Category category)
        {
            return Task.Run(async () =>
            {
                var id = _http.HttpContext.GetUserId();
                var user = await _users.FindByIdAsync(id);

                category.Owner = user;
                var entity = await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return entity.Entity;
            });
        }

        public Task<bool> RemoveCategoryAsync(Category category)
        {
            return Task.Run(async () =>
            {
                var result = _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return result != null;
            });
        }

        public Task<bool> RemoveCategoryAsync(Expression<Func<Category, bool>> predicate)
        {
            return Task.Run(async () =>
            {
                var userId = _http.HttpContext.GetUserId();
                var entry = await _context.Categories.Where(
                    x => x.OwnerId.Equals(userId)).FirstOrDefaultAsync(predicate);

                if (entry == null) return false;

                var result = _context.Categories.Remove(entry);
                await _context.SaveChangesAsync();

                return result != null;
            });
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return Task.Run(() =>
            {
                var id = _http.HttpContext.GetUserId();
                return _context.Categories.Where(x => x.OwnerId.Equals(id)).AsEnumerable();
            });
        }

        public Task<Category> UpdateCategoryAsync(Category category)
        {
            return Task.Run(async () =>
            {
                var id = _http.HttpContext.GetUserId();
                var entry =
                    await _context.Categories.FirstOrDefaultAsync(x =>
                        x.OwnerId.Equals(id) && x.Id.Equals(category.Id));

                if (entry == null) return null;

                entry.Name = category.Name;
                
                var entity = _context.Categories.Update(entry);
                await _context.SaveChangesAsync();
                return entity.Entity;
            });
        }

        public Task<Category> GetCategoryAsync(string id)
        {
            return Task.Run(async () =>
            {
                var userId = _http.HttpContext.GetUserId();
                var category =
                    await _context.Categories.FirstOrDefaultAsync(x => x.OwnerId.Equals(userId) && x.Id.Equals(id));
                return category;
            });
        }

        public Task<Category> GetCategoryAsync(Expression<Func<Category, bool>> predicate)
        {
            return Task.Run(async () =>
            {
                var userId = _http.HttpContext.GetUserId();
                if (predicate == null) return null;

                var categories = _context.Categories.Where(x => x.OwnerId.Equals(userId));
                if (!categories.Any()) return null;

                var category = await categories.FirstOrDefaultAsync(predicate);
                return category;
            });
        }
    }
}