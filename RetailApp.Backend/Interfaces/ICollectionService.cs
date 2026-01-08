using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface ICollectionService // Interfaz para el servicio de Colecciones
    {
        Task<IEnumerable<Collection>> GetAllCollectionsAsync(); // Método para obtener todas las colecciones
        Task<Collection?> GetCollectionByIdAsync(int id); // Método para obtener una colección por ID
        Task<Collection> CreateCollectionAsync(Collection collection); // Método para crear una nueva colección
        Task<bool> UpdateCollectionAsync(Collection collection); // Método para actualizar una colección existente
        Task<bool> DeleteCollectionAsync(int id); // Método para eliminar una colección por ID
    }
}
