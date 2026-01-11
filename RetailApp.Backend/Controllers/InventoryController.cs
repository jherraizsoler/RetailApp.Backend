using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/Inventory/warehouse
        [HttpGet("warehouse")]
        public async Task<ActionResult<IEnumerable<WarehouseProduct>>> GetWarehouseStock()
        {
            // Aprovechamos IQueryable y ejecución diferida
            return Ok(await _inventoryService.GetWarehouseStock().ToListAsync());
        }

        // GET: api/Inventory/store
        [HttpGet("store")]
        public async Task<ActionResult<IEnumerable<StoreProduct>>> GetStoreStock()
        {
            return Ok(await _inventoryService.GetStoreStock().ToListAsync());
        }

        // GET: api/Inventory/total/5
        [HttpGet("total/{productId}")]
        public async Task<ActionResult<int>> GetTotalStock(int productId)
        {
            var total = await _inventoryService.GetTotalProductStockAsync(productId);
            return Ok(total);
        }

        // POST: api/Inventory/warehouse/update
        [HttpPost("warehouse/update")]
        public async Task<IActionResult> UpdateWarehouseStock(int warehouseId, int productId, int quantity)
        {
            var result = await _inventoryService.UpdateWarehouseStockAsync(warehouseId, productId, quantity);
            if (!result) return BadRequest("No se pudo actualizar el stock del almacén.");
            return Ok(new { Message = "Stock de almacén actualizado correctamente." });
        }

        // POST: api/Inventory/transfer
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferToStore(int warehouseId, int storeId, int productId, int quantity)
        {
            // Nota: Este método ahora validará stock en Warehouse y asegurará la relación en Store
            var result = await _inventoryService.TransferStockToStoreAsync(warehouseId, storeId, productId, quantity);

            if (!result) return BadRequest("Error en la transferencia: Stock insuficiente o IDs inválidos.");

            return Ok(new { Message = "Transferencia completada con éxito." });
        }

        // POST: api/Inventory/purchase
        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase(int warehouseId, int productId, int quantity, decimal cost)
        {
            var result = await _inventoryService.PurchaseToWarehouseAsync(warehouseId, productId, quantity, cost);

            if (!result) return BadRequest("Error al procesar la compra de mercancía.");

            return Ok(new
            {
                Message = "Compra registrada. Stock incrementado.",
                WarehouseId = warehouseId,
                NewQuantity = quantity
            });
        }
    }
}