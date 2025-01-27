using InventoryManagement.Entities;
using InventoryManagement.Configurations;
using MongoDB.Driver;

namespace InventoryManagement.Repositories
{
    public class ProductRepository
    {
        private readonly MongoDbContext _dbContext; // Dependency injection for MongoDB context.

        public ProductRepository(MongoDbContext dbContext)
        {
            _dbContext = dbContext; // Initializes the MongoDB context for repository operations.
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            // Retrieves all products from the MongoDB collection.
            return await _dbContext.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            // Finds a product by its ID in the MongoDB collection.
            return await _dbContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            // Inserts a new product into the MongoDB collection.
            await _dbContext.Products.InsertOneAsync(product);
        }

        public async Task UpdateProductAsync(int id, Product updatedProduct)
        {
            // Replaces an existing product with updated details, identified by its ID.
            await _dbContext.Products.ReplaceOneAsync(p => p.Id == id, updatedProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            // Deletes a product from the MongoDB collection based on its ID.
            await _dbContext.Products.DeleteOneAsync(p => p.Id == id);
        }
    }
}
