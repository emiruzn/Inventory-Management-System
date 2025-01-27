using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Entities;
using InventoryManagement.Services;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // Endpoint for adding a product. Requires "ManagerPolicy" authorization. POST /api/products
        [Authorize(Policy = "ManagerPolicy")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // Endpoint for updating a product. Requires "ManagerPolicy" authorization. PUT /api/products/{id}
        [Authorize(Policy = "ManagerPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            await _productService.UpdateProductAsync(id, product);
            return NoContent();
        }

        // Endpoint for deleting a product. Requires "ManagerPolicy" authorization. DELETE /api/products/{id}
        [Authorize(Policy = "ManagerPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        // Endpoint for getting a product by ID. Requires "ViewerPolicy" authorization. GET /api/products/{id}
        [Authorize(Policy = "ViewerPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // Endpoint for getting all products. Requires "ViewerPolicy" authorization. GET /api/products
        [Authorize(Policy = "ViewerPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
    }
}
