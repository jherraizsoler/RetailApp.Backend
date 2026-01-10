using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface IProductService // Interfaz para el servicio de Productos
    {
        IQueryable<Product> GetAllProducts();// Método para obtener todos los productos
        Task<Product?> GetProductByIdAsync(int id); // Método para obtener un producto por ID
        Task<Product> CreateProductAsync(Product product); // Método para crear un nuevo producto
        Task<bool> UpdateProductAsync(Product product); // Método para actualizar un producto existente
        Task<bool> DeleteProductAsync(int id); // Método para eliminar un producto por ID
    }
}
