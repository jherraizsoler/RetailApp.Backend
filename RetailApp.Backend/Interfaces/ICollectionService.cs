using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface ICollectionService
    {
        // CRUD de Colecciones
        IQueryable<Collection> GetAllCollections();
        Task<Collection?> GetCollectionBySlugAsync(string slug);
        Task<Collection> CreateCollectionAsync(Collection collection);
        Task<bool> UpdateCollectionAsync(Collection collection);
        Task<bool> DeleteCollectionAsync(int id);

        // Gestión de Productos en la Colección
        Task<bool> AddProductToCollectionAsync(int collectionId, int productId);
        Task<bool> RemoveProductFromCollectionAsync(int collectionId, int productId);
        IQueryable<Product> GetProductsInCollection(string slug);
    }
}
