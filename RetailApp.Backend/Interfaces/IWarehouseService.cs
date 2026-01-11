// IWarehouseService.cs
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface IWarehouseService
    {
        IQueryable<Warehouse> GetAllWarehouses();
        Task<Warehouse?> GetWarehouseByIdAsync(int id);
        Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse);
        Task<bool> UpdateWarehouseAsync(Warehouse warehouse);
        Task<bool> DeleteWarehouseAsync(int id);
    }
}