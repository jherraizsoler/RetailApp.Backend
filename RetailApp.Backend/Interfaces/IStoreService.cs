// IStoreService.cs
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface IStoreService
    {
        IQueryable<Store> GetAllStores();
        Task<Store?> GetStoreByIdAsync(int id);
        Task<Store> CreateStoreAsync(Store store);
        Task<bool> UpdateStoreAsync(Store store);
        Task<bool> DeleteStoreAsync(int id);
    }
}
