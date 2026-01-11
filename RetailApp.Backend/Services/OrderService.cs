using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> CreateOrderAsync(int userId, int storeId, List<CartItem> items)
        {
            if (items == null || !items.Any()) return null;

            // Iniciamos transacción ACID para garantizar integridad
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatus = "Pending",
                    TotalAmount = 0,
                    OrderItems = new List<OrderItem>()
                };

                foreach (var item in items)
                {
                    // 1. Cargar la variante e incluir el producto base para acceder a BasePrice
                    var variant = await _context.ProductVariants
                        .Include(v => v.Product)
                        .FirstOrDefaultAsync(v => v.Id == item.ProductVariantId);

                    if (variant == null || !variant.IsActive)
                        throw new Exception($"La variante con ID {item.ProductVariantId} no está disponible.");

                    // 2. Lógica de Precios en Cascada
                    var storeProduct = await _context.StoreProducts
                        .FirstOrDefaultAsync(sp => sp.StoreId == storeId && sp.ProductId == variant.Product.Id);

                    decimal priceAtPurchase = 0;

                    if (storeProduct != null && storeProduct.PriceStore > 0)
                        priceAtPurchase = storeProduct.PriceStore; // Prioridad 1: Tienda
                    else if (variant.FinalPrice.HasValue && variant.FinalPrice > 0)
                        priceAtPurchase = variant.FinalPrice.Value; // Prioridad 2: Variante
                    else
                        priceAtPurchase = variant.Product.BasePrice; // Prioridad 3: Producto Base

                    // 3. Gestión de Stock (WarehouseProduct)
                    var inventory = await _context.WarehouseProducts
                        .FirstOrDefaultAsync(wp => wp.ProductId == variant.Product.Id);

                    if (inventory == null || inventory.StockQuantity < item.Quantity)
                        throw new Exception($"Stock insuficiente para {variant.Product.Name} - SKU: {variant.SKUCode}");

                    // Descontar del almacén
                    inventory.StockQuantity -= item.Quantity;
                    inventory.LastUpdated = DateTime.UtcNow;

                    // 4. Crear el detalle del pedido
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductVariantId = variant.Id,
                        Quantity = item.Quantity,
                        PriceAtPurchase = priceAtPurchase
                    });

                    order.TotalAmount += (priceAtPurchase * item.Quantity);
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Si todo ha ido bien, confirmamos la transacción
                await transaction.CommitAsync();
                return order;
            }
            catch (Exception)
            {
                // En caso de cualquier error (ej. falta de stock), se deshacen todos los cambios
                await transaction.RollbackAsync();
                return null;
            }
        }

        // Consultas optimizadas usando IQueryable
        public IQueryable<Order> GetUserOrders(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .AsNoTracking() // Rendimiento de lectura
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task<Order?> GetOrderDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(v => v.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}