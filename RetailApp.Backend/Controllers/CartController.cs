using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/Cart/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCart(int userId)
        {
            // 1. Obtenemos el IQueryable filtrado por usuario
            var query = _cartService.GetCartItems(userId);

            // 2. Aplicamos Eager Loading para traer la variante y el producto base
            // Esto es posible gracias a que el servicio retorna IQueryable
            var items = await query
                .Include(ci => ci.ProductVariant)
                    .ThenInclude(pv => pv.Product)
                .ToListAsync();

            if (items == null || !items.Any())
            {
                return Ok(new List<CartItem>()); // Retornamos lista vacía si no hay ítems
            }

            return Ok(items);
        }

        // POST: api/Cart/5
        [HttpPost("{userId}")]
        public async Task<ActionResult<CartItem>> AddToCart(int userId, [FromBody] CartItem item)
        {
            if (item.Quantity <= 0)
            {
                return BadRequest("La cantidad debe ser mayor a cero.");
            }

            var createdItem = await _cartService.AddCartItemAsync(userId, item);

            return CreatedAtAction(nameof(GetCart), new { userId = userId }, createdItem);
        }

        // DELETE: api/Cart/5/item/10
        [HttpDelete("{userId}/item/{itemId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int itemId)
        {
            var success = await _cartService.RemoveCartItemAsync(userId, itemId);

            if (!success)
            {
                return NotFound("El ítem no existe en el carrito del usuario.");
            }

            return NoContent();
        }

        // DELETE: api/Cart/5/clear
        [HttpDelete("{userId}/clear")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }
    }
}