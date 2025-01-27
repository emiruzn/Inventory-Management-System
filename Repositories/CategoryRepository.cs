using InventoryManagement.Entities;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Repositories
{
    public class CategoryRepository
    {
        private readonly AppDbContext _context; // Dependency injection for the database context.

        public CategoryRepository(AppDbContext context)
        {
            _context = context; // Constructor initializes the context for use in repository methods.
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            // Fetches all categories from the database asynchronously.
            return await _context.Set<Category>().ToListAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            // Adds a new category entity to the database.
            await _context.Set<Category>().AddAsync(category);
            // Saves changes to the database asynchronously.
            await _context.SaveChangesAsync();
        }
    }
}
