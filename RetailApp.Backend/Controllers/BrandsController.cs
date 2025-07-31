using Microsoft.AspNetCore.Mvc;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailApp.Backend.Controllers
{
    [Route("api/[controller]")] // Defines the base route for the controller (e.g., /api/Brands)
    [ApiController] // Indicates that this controller is an API Controller
    public class BrandsController : ControllerBase // Inherits from ControllerBase to be an API controller
    {
        private readonly IBrandService _brandService; // Interface for brand service
        // Constructor that injects the brand service
        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        // GET: /api/Brands
        [HttpGet] // Defines the HTTP GET method for this action
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands); // Returns a 200 OK response with the list of brands
        }

        // GET: /api/Brands/5
        [HttpGet("{id}")] // Defines the HTTP GET method to get a brand by ID
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound(); // Returns 404 Not Found if the brand does not exist
            }
            return Ok(brand); // Returns 200 OK with the found brand
        }

        // POST: /api/Brands
        [HttpPost] // Defines the HTTP POST method to create a new brand
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            var createdBrand = await _brandService.CreateBrandAsync(brand);
            return CreatedAtAction(nameof(GetBrand), new { id = createdBrand.Id }, createdBrand); // Returns 201 Created with the location of the new brand
        }

        // PUT: /api/Brands/5
        [HttpPut("{id}")] // Defines the HTTP PUT method to update a brand by ID
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.Id)
            {
                return BadRequest(); // Returns 400 Bad Request if IDs do not match
            }
            var success = await _brandService.UpdateBrandAsync(brand);
            if (!success)
            {
                return NotFound(); // Returns 404 Not Found if the brand does not exist
            }
            return NoContent(); // Returns 204 No Content on successful update
        }


        // DELETE: /api/Brands/5
        [HttpDelete("{id}")] // Defines the HTTP DELETE method to delete a brand by ID
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var success = await _brandService.DeleteBrandAsync(id);
            if (!success)
            {
                return NotFound(); // Returns 404 Not Found if the brand does not exist
            }
            return NoContent(); // Returns 204 No Content on successful deletion
        }

    }
}
