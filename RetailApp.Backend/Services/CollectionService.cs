using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Linq; // Agrega esto para usar .Any()
using System.Threading.Tasks;

namespace RetailApp.Backend.Services 
{
    public class CollectionService : ICollectionService // Implement ICollectionService interface
    {
        private readonly ApplicationDbContext _context;
        public CollectionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Collection>> GetAllCollectionsAsync()
        {
            // Obtiene todas las colecciones
            return await _context.Collections.ToListAsync();
        }
        public async Task<Collection?> GetCollectionByIdAsync(int id)
        {
            // Obtener una colección por su ID
            return await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Collection> CreateCollectionAsync(Collection collection)
        {
            // Añadir una nueva colección a la base de datos
            _context.Collections.Add(collection);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return collection; // Retorna la colección creada (Con su ID generado)
        }
        public async Task<bool> UpdateCollectionAsync(Collection collection)
        {
            // Marcar la colección como modificada
            _context.Entry(collection).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true; // Retorna true si la actualización fue exitosa
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejar si la colección no existe para actualizar
                if (!_context.Collections.Any(e => e.Id == collection.Id))
                {
                    return false; // Retorna false si la colección no existe
                }
                throw; // Vuelve a lanzar la excepción si es otro tipo de error
            }
        }
        public async Task<bool> DeleteCollectionAsync(int id)
        {
            var collection = await GetCollectionByIdAsync(id);
            if (collection == null) return false; // Si no se encuentra la colección, retorna false
            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return true; // Retorna true si la eliminación fue exitosa
        }
    }
}
