using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface IInventoryService
    {
        // Consultas de Stock
        IQueryable<WarehouseProduct> GetWarehouseStock();
        IQueryable<StoreProduct> GetStoreStock();
        Task<int> GetTotalProductStockAsync(int productId);

        // Operaciones de Movimiento
        Task<bool> UpdateWarehouseStockAsync(int warehouseId, int productId, int quantity);
        Task<bool> UpdateStoreStockAsync(int storeId, int productId, int quantity);
        Task<bool> TransferStockToStoreAsync(int warehouseId, int storeId, int productId, int quantity);

        // Añadir a la interfaz IInventoryService.cs
        Task<bool> PurchaseToWarehouseAsync(int warehouseId, int productId, int quantity, decimal acquisitionCost);
    }
}