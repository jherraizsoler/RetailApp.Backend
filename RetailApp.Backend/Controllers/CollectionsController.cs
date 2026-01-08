using Microsoft.AspNetCore.Mvc;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailApp.Backend.Controllers
{
    [Route("api/[controller]")] // Defines the base route for the controller (e.g., /api/Collections)
    [ApiController] // Indicates that this controller is an API Controller
    public class CollectionsController : ControllerBase // Inherits from ControllerBase to be an API controller
    {
        private readonly ICollectionService _collectionService; // Interface for collection service
        // Constructor that injects the collection service
        public CollectionsController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }
        // GET: /api/Collections
        [HttpGet] // Defines the HTTP GET method for this action
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollections()
        {
            var collections = await _collectionService.GetAllCollectionsAsync();
            return Ok(collections); // Returns a 200 OK response with the list of collections
        }
        // GET: /api/Collections/5
        [HttpGet("{id}")] // Defines the HTTP GET method to get a collection by ID
        public async Task<ActionResult<Collection>> GetCollection(int id)
        {
            var collection = await _collectionService.GetCollectionByIdAsync(id);
            if (collection == null)
            {
                return NotFound(); // Returns 404 Not Found if the collection does not exist
            }
            return Ok(collection); // Returns 200 OK with the found collection
        }
        // POST: /api/Collections
        [HttpPost] // Defines the HTTP POST method to create a new collection
        public async Task<ActionResult<Collection>> PostCollection(Collection collection)
        {
            var createdCollection = await _collectionService.CreateCollectionAsync(collection);
            return CreatedAtAction(nameof(GetCollection), new { id = createdCollection.Id }, createdCollection); // Returns 201 Created with the location of the new collection
        }
        // PUT: /api/Collections/5
        [HttpPut("{id}")] // Defines the HTTP PUT method to update a collection by ID
        public async Task<IActionResult> PutCollection(int id, Collection collection)
        {
            if (id != collection.Id)
            {
                return BadRequest(); // Returns 400 Bad Request if IDs do not match
            }
            var success = await _collectionService.UpdateCollectionAsync(collection);
            if (!success)
            {
                return NotFound(); // Returns 404 Not Found if the collection does not exist
            }
            return NoContent(); // Returns 204 No Content on successful update
        }
        // DELETE: /api/Collections/5
        [HttpDelete("{id}")] // Defines the HTTP DELETE method to delete a collection by ID
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var success = await _collectionService.DeleteCollectionAsync(id);
            if (!success)
            {
                return NotFound(); // Returns 404 Not Found if the collection does not exist
            }
            return NoContent(); // Returns 204 No Content on successful deletion
        }
    }
}
