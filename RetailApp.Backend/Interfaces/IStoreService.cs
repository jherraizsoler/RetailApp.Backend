using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface IStoreService // Interfaz para el servicio de Tiendas
    {
        Task<IEnumerable<Store>> GetAllStoresAsync(); // Método para obtener todas las tiendas
        Task<Store?> GetStoreByIdAsync(int id); // Método para obtener una tienda por ID
        Task<Store> CreateStoreAsync(Store store); // Método para crear una nueva tienda
        Task<bool> UpdateStoreAsync(Store store); // Método para actualizar una tienda existente
        Task<bool> DeleteStoreAsync(int id); // Método para eliminar una tienda por ID
    }
}
