using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Services
{
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;

        public StoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ REFACTOR: Retornamos IQueryable y usamos AsNoTracking
        public IQueryable<Store> GetAllStores()
        {
            return _context.Stores.AsNoTracking();
        }

        public async Task<Store?> GetStoreByIdAsync(int id)
        {
            return await _context.Stores
                .Include(s => s.StoreProducts)!
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Store> CreateStoreAsync(Store store)
        {
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<bool> UpdateStoreAsync(Store store)
        {
            _context.Entry(store).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // ✅ REFACTOR: Any() es más rápido que buscar el objeto completo
                return _context.Stores.Any(e => e.Id == store.Id) ? throw : false;
            }
        }

        // ✅ REFACTOR: Implementación de Stub Delete (Sin SELECT previo)
        public async Task<bool> DeleteStoreAsync(int id)
        {
            var store = new Store { Id = id };
            _context.Entry(store).State = EntityState.Deleted;

            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false; // La tienda no existe
            }
        }
    }
}