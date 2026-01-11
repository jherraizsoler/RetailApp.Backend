using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;
        public InventoryService(ApplicationDbContext context) => _context = context;

        public IQueryable<WarehouseProduct> GetWarehouseStock() =>
            _context.WarehouseProducts.Include(wp => wp.Product).AsNoTracking();

        public IQueryable<StoreProduct> GetStoreStock() =>
            _context.StoreProducts.Include(sp => sp.Product).AsNoTracking();

        public async Task<int> GetTotalProductStockAsync(int productId)
        {
            var wStock = await _context.WarehouseProducts
                .Where(wp => wp.ProductId == productId).SumAsync(wp => wp.StockQuantity);

            var sStock = await _context.StoreProducts
                .Where(sp => sp.ProductId == productId).SumAsync(sp => sp.StockQuantity);

            return wStock + sStock;
        }

        public async Task<bool> UpdateWarehouseStockAsync(int warehouseId, int productId, int quantity)
        {
            var stock = await _context.WarehouseProducts
                .FirstOrDefaultAsync(wp => wp.WarehouseId == warehouseId && wp.ProductId == productId);

            if (stock == null)
            {
                _context.WarehouseProducts.Add(new WarehouseProduct
                {
                    WarehouseId = warehouseId,
                    ProductId = productId,
                    StockQuantity = quantity,
                    LastUpdated = DateTime.UtcNow
                });
            }
            else
            {
                stock.StockQuantity = quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        // ✅ Implementación de UpdateStoreStockAsync (Completada)
        public async Task<bool> UpdateStoreStockAsync(int storeId, int productId, int quantity)
        {
            var stock = await _context.StoreProducts
                .FirstOrDefaultAsync(sp => sp.StoreId == storeId && sp.ProductId == productId);

            if (stock == null)
            {
                _context.StoreProducts.Add(new StoreProduct
                {
                    StoreId = storeId,
                    ProductId = productId,
                    StockQuantity = quantity,
                    LastUpdated = DateTime.UtcNow
                });
            }
            else
            {
                stock.StockQuantity = quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        // ✅ Transferencia con Integridad Transaccional y Lógica de Upsert en Destino
        public async Task<bool> TransferStockToStoreAsync(int warehouseId, int storeId, int productId, int quantity)
        {
            if (quantity <= 0) return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Validar y restar del almacén
                var wStock = await _context.WarehouseProducts
                    .FirstOrDefaultAsync(wp => wp.WarehouseId == warehouseId && wp.ProductId == productId);

                if (wStock == null || wStock.StockQuantity < quantity)
                    return false;

                wStock.StockQuantity -= quantity;
                wStock.LastUpdated = DateTime.UtcNow;

                // 2. Sumar a la tienda (Con lógica de creación si no existe)
                var sStock = await _context.StoreProducts
                    .FirstOrDefaultAsync(sp => sp.StoreId == storeId && sp.ProductId == productId);

                if (sStock == null)
                {
                    _context.StoreProducts.Add(new StoreProduct
                    {
                        StoreId = storeId,
                        ProductId = productId,
                        StockQuantity = quantity,
                        LastUpdated = DateTime.UtcNow
                    });
                }
                else
                {
                    sStock.StockQuantity += quantity;
                    sStock.LastUpdated = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        // Método para registrar compras (Inbound) al almacén
        public async Task<bool> PurchaseToWarehouseAsync(int warehouseId, int productId, int quantity, decimal acquisitionCost)
        {
            if (quantity <= 0) return false;

            // Usamos transacción para asegurar que la entrada de stock sea atómica
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var stock = await _context.WarehouseProducts
                    .FirstOrDefaultAsync(wp => wp.WarehouseId == warehouseId && wp.ProductId == productId);

                if (stock == null)
                {
                    _context.WarehouseProducts.Add(new WarehouseProduct
                    {
                        WarehouseId = warehouseId,
                        ProductId = productId,
                        StockQuantity = quantity, // Cantidad comprada
                        LastUpdated = DateTime.UtcNow
                    });
                }
                else
                {
                    stock.StockQuantity += quantity; // Incremento de stock existente
                    stock.LastUpdated = DateTime.UtcNow;
                }

                // Aquí es donde en el futuro registrarías 'acquisitionCost' en una tabla de 'PurchaseHistory'

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}