using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface ICartService // Interfaz para el servicio de Carrito de Compras
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId); // Método para obtener los artículos del carrito de un usuario
        Task<CartItem?> GetCartItemByIdAsync(int userId, int itemId); // Método para obtener un artículo del carrito por ID
        Task<CartItem> AddCartItemAsync(int userId, CartItem cartItem); // Método para agregar un artículo al carrito
        Task<bool> UpdateCartItemAsync(CartItem cartItem); // Método para actualizar un artículo en el carrito
        Task<bool> RemoveCartItemAsync(int userId, int itemId); // Método para eliminar un artículo del carrito por ID
        Task ClearCartAsync(int userId); // Método para vaciar el carrito de un usuario
    }
}
