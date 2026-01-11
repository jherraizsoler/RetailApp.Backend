using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/Orders/checkout
        // Este endpoint recibe la intención de compra
        [HttpPost("checkout")]
        public async Task<ActionResult<Order>> CreateOrder(int userId, int storeId, [FromBody] List<CartItem> items)
        {
            if (items == null || !items.Any())
            {
                return BadRequest(new { Message = "El carrito está vacío." });
            }

            var order = await _orderService.CreateOrderAsync(userId, storeId, items);

            if (order == null)
            {
                return BadRequest(new { Message = "No se pudo procesar la orden. Verifique disponibilidad de stock o precios." });
            }

            // Retornamos 201 Created y la ubicación de la nueva orden
            return CreatedAtAction(nameof(GetOrderDetails), new { id = order.Id }, order);
        }

        // GET: api/Orders/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(int userId)
        {
            var orders = await _orderService.GetUserOrders(userId).ToListAsync();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderDetails(int id)
        {
            var order = await _orderService.GetOrderDetailsAsync(id);

            if (order == null)
            {
                return NotFound(new { Message = $"Orden con ID {id} no encontrada." });
            }

            return Ok(order);
        }
    }
}