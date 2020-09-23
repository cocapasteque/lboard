using System.Threading.Tasks;
using LBoard.Models.Leaderboard;
using LBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categories;
        private readonly ILogger<CategoriesController> _logger;
        
        public CategoriesController(ILogger<CategoriesController> logger, ICategoryService categories)
        {
            _logger = logger;
            _categories = categories;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categories.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCategory(string name)
        {
            var category = await _categories.GetCategoryAsync(x => x.Name.Equals(name));
            return Ok(category);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> RemoveCategory(string name)
        {
            var category = await _categories.RemoveCategoryAsync(x => x.Name.Equals(name));
            return Ok($"{(category ? "Success" : "Failure")}");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            var result = await _categories.UpdateCategoryAsync(category);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            var result = await _categories.AddCategoryAsync(category);
            return Ok(result);
        }
    }
}