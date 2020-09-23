using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LBoard.Models.Leaderboard;

namespace LBoard.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> AddCategoryAsync(Category category);
        Task<bool> RemoveCategoryAsync(Category category);
        Task<bool> RemoveCategoryAsync(Expression<Func<Category, bool>> predicate);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> UpdateCategoryAsync(Category category);
        Task<Category> GetCategoryAsync(string id);
        Task<Category> GetCategoryAsync(Expression<Func<Category, bool>> predicate);
    }
}