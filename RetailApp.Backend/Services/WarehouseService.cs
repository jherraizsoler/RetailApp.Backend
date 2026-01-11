using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Services
{
    public class WarehouseService : IWarehouseService // Implementación de alto rendimiento
    {
        private readonly ApplicationDbContext _context;

        public WarehouseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ IQueryable + AsNoTracking: El filtrado ocurre en SQL, no en RAM
        public IQueryable<Warehouse> GetAllWarehouses()
        {
            return _context.Warehouses.AsNoTracking();
        }

        public async Task<Warehouse?> GetWarehouseByIdAsync(int id)
        {
            // Cargamos las relaciones necesarias para ver el stock del almacén
            return await _context.Warehouses
                .Include(w => w.WarehouseProducts)!
                .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }

        public async Task<bool> UpdateWarehouseAsync(Warehouse warehouse)
        {
            _context.Entry(warehouse).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificamos si el error es porque el registro ya no existe
                if (!_context.Warehouses.Any(e => e.Id == warehouse.Id))
                {
                    return false;
                }
                else
                {
                    // Si existe pero hubo otro error de concurrencia, relanzamos la excepción
                    throw;
                }
            }
        }

        // ✅ Stub Delete: Eliminación en un solo paso (Sin SELECT previo)
        public async Task<bool> DeleteWarehouseAsync(int id)
        {
            var warehouse = new Warehouse { Id = id };
            _context.Entry(warehouse).State = EntityState.Deleted;

            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                // El almacén no existía o ya fue borrado
                return false;
            }
        }
    }
}
