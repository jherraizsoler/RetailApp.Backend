using Microsoft.AspNetCore.Mvc;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailApp.Backend.Controllers
{

    [Route("api/[controller]")] // /api/stores
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;
        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // GET: api/stores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            var stores = await _storeService.GetAllStoresAsync();
            return Ok(stores);
        }

        // GET: api/stores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _storeService.GetStoreByIdAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }

        // POST: api/stores
        [HttpPost]
        public async Task<ActionResult<Store>> PostStore([FromBody] Store store)
        {
            var createdStore = await _storeService.CreateStoreAsync(store);
            return CreatedAtAction(nameof(GetStore), new { id = createdStore.Id }, createdStore);
        }

        // PUT: api/stores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, Store store)
        {
            if (id != store.Id)
            {
                return BadRequest();
            }
            var success = await _storeService.UpdateStoreAsync(store);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/stores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var success = await _storeService.DeleteStoreAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
