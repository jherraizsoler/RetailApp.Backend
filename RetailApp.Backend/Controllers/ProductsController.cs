using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailApp.Backend.Controllers
{
    [Route("api/[controller]")] // Define the base route for the controller (e.g., /api/Products)
    [ApiController] // Indicates that this controller is an API Controller
    public class ProductsController : ControllerBase // Inherits from ControllerBase to be an API controller
    {
        private readonly IProductService _productService; // Interface for product service
        // Constructor that injects the product service
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        
        
        // GET: /api/Products
        // Method to get all products
        [HttpGet] // Defines the HTTP GET method for this action
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] int? categoryId,
        [FromQuery] int? brandId,
        [FromQuery] string? search)
        {
            // 1. Iniciamos la consulta diferida (IQueryable)
            var query = _productService.GetAllProducts();

            // 2. Filtros dinámicos en el servidor SQL
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            // 3. Eager Loading: Traemos la Marca y Categoría en un solo JOIN
            // 4. Materialización: Ejecutamos el SQL final
            var products = await query
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();

            return Ok(products);
        }


        // GET: /api/Products/5
        [HttpGet("{id}")] // Defines the HTTP GET method to get a product by ID
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Returns 404 Not Found if the product does not exist
            }
            return Ok(product); // Returns 200 OK with the found product
        }
        
        
        // POST: /api/Products
        [HttpPost] // Defines the HTTP POST method to create a new product
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct); // Returns 201 Created with the location of the new product
        }
        
        
        // PUT: /api/Products/5
        [HttpPut("{id}")] // Defines the HTTP PUT method to update a product by ID
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest(); // Returns 400 Bad Request if IDs do not match
            }
            var success = await _productService.UpdateProductAsync(product);

            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // Returns 204 No Content on successful update
        }


        // DELETE: /api/Products/5
        [HttpDelete("{id}")] // Defines the HTTP DELETE method to delete a product by ID
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound(); // Returns 404 Not Found if the product does not exist
            }
            return NoContent(); // Returns 204 No Content on successful deletion
        }
    }
}
