using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly ApplicationDbContext _context;

        public CollectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Collection> GetAllCollections() =>
            _context.Collections.AsNoTracking().OrderByDescending(c => c.CreatedDate);

        // ✅ Implementado: Búsqueda por Slug para URLs amigables
        public async Task<Collection?> GetCollectionBySlugAsync(string slug) =>
            await _context.Collections
                .Include(c => c.Products!)
                .FirstOrDefaultAsync(c => c.Name.ToLower().Replace(" ", "-") == slug.ToLower());

        public async Task<Collection> CreateCollectionAsync(Collection collection)
        {
            collection.CreatedDate = DateTime.UtcNow;
            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();
            return collection;
        }

        // ✅ Implementado: Actualización con manejo de estado
        public async Task<bool> UpdateCollectionAsync(Collection collection)
        {
            _context.Entry(collection).State = EntityState.Modified;
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si hay un error de concurrencia, verificamos si es porque el registro ya no existe
                if (!_context.Collections.Any(e => e.Id == collection.Id))
                {
                    return false; // El registro fue eliminado por otro proceso
                }
                else
                {
                    throw; // Fue otro error de concurrencia, relanzamos la excepción
                }
            }
        }

        public async Task<bool> DeleteCollectionAsync(int id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection == null) return false;

            _context.Collections.Remove(collection);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddProductToCollectionAsync(int collectionId, int productId)
        {
            var collection = await _context.Collections.Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == collectionId);
            var product = await _context.Products.FindAsync(productId);

            if (collection == null || product == null) return false;

            if (!collection.Products!.Any(p => p.Id == productId))
            {
                collection.Products.Add(product);
                return await _context.SaveChangesAsync() > 0;
            }
            return true;
        }

        // ✅ Implementado: Quitar producto de la colección
        public async Task<bool> RemoveProductFromCollectionAsync(int collectionId, int productId)
        {
            var collection = await _context.Collections.Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == collectionId);

            if (collection == null) return false;

            var product = collection.Products!.FirstOrDefault(p => p.Id == productId);
            if (product == null) return false;

            collection.Products!.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        // ✅ Implementado: Obtener productos por Slug (Eficiencia IQueryable)
        public IQueryable<Product> GetProductsInCollection(string slug)
        {
            return _context.Collections
                .Where(c => c.Name.ToLower().Replace(" ", "-") == slug.ToLower())
                .SelectMany(c => c.Products!)
                .Include(p => p.Variants)
                .AsNoTracking();
        }
    }
}