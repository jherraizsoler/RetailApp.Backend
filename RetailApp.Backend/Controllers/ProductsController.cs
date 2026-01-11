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
        public async Task<ActionResult> GetProducts(
            [FromQuery] int? categoryId,
            [FromQuery] int? brandId,
            [FromQuery] string? search,
            [FromQuery] string? slug, // Nuevo filtro SEO
            [FromQuery] bool onlyActive = true)
        {
            // 1. Iniciamos la consulta diferida
            var query = _productService.GetAllProducts();

            // 2. Filtros dinámicos
            if (onlyActive) query = query.Where(p => p.IsActive);

            if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId.Value);

            if (brandId.HasValue) query = query.Where(p => p.BrandId == brandId.Value);

            if (!string.IsNullOrEmpty(slug)) query = query.Where(p => p.URL_Slug == slug);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.ShortDescription.Contains(search));
            }

            // 3. Aplicamos Proyección (.Select) para evitar cargar LongDescription y SEO pesado
            // Esto optimiza la RAM y el tráfico de red
            var products = await query
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages) // Eager Loading de imágenes
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.ShortDescription,
                    p.BasePrice,
                    p.MainImageUrl,
                    p.URL_Slug,
                    BrandName = p.Brand.Name,
                    CategoryName = p.Category.Name,
                    Images = p.ProductImages.Select(img => img.ImageUrl).ToList()
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: /api/Products/by-slug/laptop-gaming-pro
        [HttpGet("by-slug/{slug}")]
        public async Task<ActionResult<Product>> GetProductBySlug(string slug)
        {
            // Buscamos el producto por su URL única
            var product = await _productService.GetAllProducts()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Variants) // Aquí sí traemos las variantes (tallas, colores)
                .FirstOrDefaultAsync(p => p.URL_Slug == slug);

            if (product == null)
            {
                return NotFound(new { message = $"Producto con slug '{slug}' no encontrado." });
            }

            // Aquí devolvemos TODO, incluyendo la LongDescription que omitimos en el listado
            return Ok(product);
        }


        // GET: /api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // 1. Usamos el IQueryable para componer la consulta con sus relaciones
            var product = await _productService.GetAllProducts()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Variants) // Traemos las variantes para el selector de tallas/colores
                .FirstOrDefaultAsync(p => p.Id == id);

            // 2. Validación de existencia
            if (product == null)
            {
                return NotFound(new { message = $"El producto con ID {id} no existe." });
            }

            // 3. Aquí SÍ devolvemos el objeto Product completo (con LongDescription)
            // porque estamos en la vista de detalle.
            return Ok(product);
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
