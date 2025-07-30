using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailApp.Backend.Services
{
    public class StoreService : IStoreService // Implementación del servicio de Tiendas
    {
        private readonly ApplicationDbContext _context;

        public StoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<Store?> GetStoreByIdAsync(int id)
        {
            // Opcional: Incluir productos si es necesario
            return await _context.Stores
                                 .Include(s => s.StoreProducts)!
                                 .ThenInclude(sp => sp.Product) // Carga los productos asociados
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
                if (!_context.Stores.Any(e => e.Id == store.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteStoreAsync(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return false;
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}