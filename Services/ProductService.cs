using InventoryManagement.Entities;
using InventoryManagement.Repositories;
using StackExchange.Redis;

namespace InventoryManagement.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly IDatabase _cache;

        public ProductService(ProductRepository productRepository, IConnectionMultiplexer redis)
        {
            _productRepository = productRepository;
            _cache = redis.GetDatabase();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var cachedProducts = await _cache.StringGetAsync("products");
            if (!string.IsNullOrEmpty(cachedProducts))
            {
                var cachedProductList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(cachedProducts);
                return cachedProductList ?? new List<Product>();
            }

            var products = await _productRepository.GetAllProductsAsync();
            await _cache.StringSetAsync("products", Newtonsoft.Json.JsonConvert.SerializeObject(products));
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var cachedProduct = await _cache.StringGetAsync($"product:{id}");
            if (!string.IsNullOrEmpty(cachedProduct))
            {
                var cachedProductObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(cachedProduct);
                return cachedProductObj ?? new Product
                {
                    Id = -1,
                    Name = string.Empty,
                    Description = string.Empty,
                    Price = (float)0.0m,
                    Quantity = 0,
                    Category = string.Empty
                };
            }

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product != null)
            {
                await _cache.StringSetAsync($"product:{id}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            }
            return product ?? new Product
            {
                Id = -1,
                Name = string.Empty,
                Description = string.Empty,
                Price = (float)0.0m,
                Quantity = 0,
                Category = string.Empty
            };
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
            await InvalidateCache();
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            await _productRepository.UpdateProductAsync(id, product);
            await InvalidateCache();
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
            await InvalidateCache();
        }

        private async Task InvalidateCache()
        {
            await _cache.KeyDeleteAsync("products");
        }
    }
}
