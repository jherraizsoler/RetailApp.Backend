using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface IOrderService
    {
        // El corazón del sistema: Transforma un carrito en una orden legal
        Task<Order?> CreateOrderAsync(int userId, int storeId, List<CartItem> items);

        // Consultas optimizadas con IQueryable para Big Data
        IQueryable<Order> GetUserOrders(int userId);
        Task<Order?> GetOrderDetailsAsync(int orderId);
    }
}