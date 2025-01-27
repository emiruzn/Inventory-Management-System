using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagement.Entities;
using InventoryManagement.Repositories;

namespace InventoryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoriesController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        
        // Endpoint for adding a category. Requires "AdminPolicy" authorization. POST /api/categories
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            await _categoryRepository.AddCategoryAsync(category);
            return Ok(new { message = "Category added successfully" });
        }

        // Endpoint for getting all categories. Requires "ViewerPolicy" authorization. GET /api/categories
        [Authorize(Policy = "ViewerPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
