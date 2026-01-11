using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailApp.Backend.Controllers
{

    [Route("api/[controller]")] // Defines the base route for the controller (e.g., /api/Categories)
    [ApiController] // Indicates that this controller is an API Controller
    public class CategoriesController : ControllerBase // Inherits from ControllerBase to be an API controller
    {
        private readonly ICategoryService _categoryService; // Interface for category service
        // Constructor that injects the category service
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET: /api/Categories
        [HttpGet] // Defines the HTTP GET method for this action
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories([FromQuery] string? search)
        {
            // 1. Iniciamos la consulta diferida
            var query = _categoryService.GetAllCategories();

            // 2. Filtro dinámico por nombre si se proporciona
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            // 3. Opcional: Podrías incluir los productos si fuera necesario
            // query = query.Include(c => c.Products);

            // 4. Ejecutamos la consulta en la base de datos
            var categories = await query.ToListAsync();

            return Ok(categories);
        }
        // GET: /api/Categories/5
        [HttpGet("{id}")] // Defines the HTTP GET method to get a category by ID
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(); // Returns 404 Not Found if the category does not exist
            }
            return Ok(category); // Returns 200 OK with the found category
        }
        // POST: /api/Categories
        [HttpPost] // Defines the HTTP POST method to create a new category
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory); // Returns 201 Created with the location of the new category
        }
        // PUT: /api/Categories/5
        [HttpPut("{id}")] // Defines the HTTP PUT method to update a category by ID
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest(); // Returns 400 Bad Request if IDs do not match
            }
            var success = await _categoryService.UpdateCategoryAsync(category);
            if (!success)
            {
                return NotFound(); // Returns 404 Not Found if the category does not exist
            }
            return NoContent(); // Returns 204 No Content on successful update
        }
        // DELETE: /api/Categories/5
        [HttpDelete("{id}")] // Defines the HTTP DELETE method to delete a category by ID
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success)
            {
                return NotFound(); // Returns 404 Not Found if the category does not exist
            }
            return NoContent(); // Returns 204 No Content on successful deletion
        }
    }
}
