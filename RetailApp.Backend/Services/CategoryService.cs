using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Linq; // Agrega esto para usar .Any()
using System.Threading.Tasks;

namespace RetailApp.Backend.Services
{
    public class CategoryService : ICategoryService // Implement ICategoryService interface
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Category> GetAllCategories()
        {
            return _context.Categories.AsNoTracking();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            // Obtener una categoría por su ID
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            // Añadir una nueva categoría a la base de datos
            _context.Categories.Add(category);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return category; // Retorna la categoría creada (Con su ID generado)
        }
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            // Marcar la categoría como modificada
            _context.Entry(category).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true; // Retorna true si la actualización fue exitosa
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejar si la categoría no existe para actualizar
                if (!_context.Categories.Any(e => e.Id == category.Id))
                {
                    return false; // Retorna false si la categoría no existe
                }
                throw; // Vuelve a lanzar la excepción si es otro tipo de error
            }
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category == null) return false; // Si no se encuentra la categoría, retorna false
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return true; // Retorna true si la eliminación fue exitosa
        }
    }
}
