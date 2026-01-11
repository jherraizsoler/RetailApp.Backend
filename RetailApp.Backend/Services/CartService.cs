using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Usamos la propiedad de navegación ci.Cart.UserId para filtrar
        public IQueryable<CartItem> GetCartItems(int userId)
        {
            return _context.CartItems
                .Where(ci => ci.Cart.UserId == userId)
                .AsNoTracking();
        }

        // 2. Corregido el acceso a UserId para buscar un ítem específico
        public async Task<CartItem?> GetCartItemByIdAsync(int userId, int itemId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.Id == itemId);
        }

        public async Task<CartItem> AddCartItemAsync(int userId, CartItem cartItem)
        {
            // Usamos el IQueryable para encontrar o crear el carrito de forma atómica
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync(); // Guardamos para obtener el ID del nuevo carrito
            }

            // Buscamos si el producto ya existe en el carrito para INCREMENTAR en lugar de DUPLICAR
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductVariantId == cartItem.ProductVariantId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity; // Sumamos la cantidad
                _context.Entry(existingItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return existingItem;
            }

            cartItem.CartId = cart.Id;
            cartItem.AddedDate = DateTime.UtcNow;
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.Entry(cartItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        // 3. Eliminación validando la propiedad del usuario a través del Cart
        public async Task<bool> RemoveCartItemAsync(int userId, int itemId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.Id == itemId);

            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Limpieza masiva filtrando por la relación
        public async Task ClearCartAsync(int userId)
        {
            var items = await _context.CartItems
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();

            if (items.Any())
            {
                _context.CartItems.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
        }
    }
}