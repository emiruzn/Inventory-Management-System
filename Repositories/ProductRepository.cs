using InventoryManagement.Entities;
using InventoryManagement.Configurations;
using MongoDB.Driver;

namespace InventoryManagement.Repositories
{
    public class ProductRepository
    {
        private readonly MongoDbContext _dbContext;

        public ProductRepository(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dbContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            await _dbContext.Products.InsertOneAsync(product);
        }

        public async Task UpdateProductAsync(int id, Product updatedProduct)
        {
            await _dbContext.Products.ReplaceOneAsync(p => p.Id == id, updatedProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _dbContext.Products.DeleteOneAsync(p => p.Id == id);
        }
    }
}
