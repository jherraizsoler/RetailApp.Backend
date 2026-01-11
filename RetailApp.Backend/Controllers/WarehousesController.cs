using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses([FromQuery] string? search)
        {
            // 1. Obtenemos el IQueryable (No se ha ejecutado SQL todavía)
            var query = _warehouseService.GetAllWarehouses();

            // 2. Componemos la consulta dinámicamente según los parámetros
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(w => w.Name.Contains(search) ||
                                         (w.City != null && w.City.Contains(search)));
            }

            // 3. Ejecución Diferida: Aquí es donde se genera y ejecuta el SQL (Select * From Warehouses WHERE...)
            var warehouses = await query.ToListAsync();

            return Ok(warehouses);
        }

        // GET: api/Warehouses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(new { Message = $"Almacén con ID {id} no encontrado." });
            }

            return Ok(warehouse);
        }

        // POST: api/Warehouses
        [HttpPost]
        public async Task<ActionResult<Warehouse>> PostWarehouse(Warehouse warehouse)
        {
            var createdWarehouse = await _warehouseService.CreateWarehouseAsync(warehouse);

            return CreatedAtAction(nameof(GetWarehouse), new { id = createdWarehouse.Id }, createdWarehouse);
        }

        // PUT: api/Warehouses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(int id, Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return BadRequest(new { Message = "El ID del almacén no coincide con el cuerpo de la solicitud." });
            }

            var success = await _warehouseService.UpdateWarehouseAsync(warehouse);

            if (!success)
            {
                return NotFound(new { Message = "No se pudo actualizar. El almacén no existe." });
            }

            return NoContent();
        }

        // DELETE: api/Warehouses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var success = await _warehouseService.DeleteWarehouseAsync(id);

            if (!success)
            {
                return NotFound(new { Message = "No se pudo eliminar. El almacén no existe." });
            }

            return NoContent();
        }
    }
}