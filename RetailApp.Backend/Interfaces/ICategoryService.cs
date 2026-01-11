using RetailApp.Backend.Models;
using System.Linq;

namespace RetailApp.Backend.Interfaces
{
    public interface ICategoryService // Interfaz para el servicio de Categorías
    {
        IQueryable<Category> GetAllCategories(); // Método para obtener todas las categorías
        Task<Category?> GetCategoryByIdAsync(int id); // Método para obtener una categoría por ID
        Task<Category> CreateCategoryAsync(Category category); // Método para crear una nueva categoría
        Task<bool> UpdateCategoryAsync(Category category); // Método para actualizar una categoría existente
        Task<bool> DeleteCategoryAsync(int id); // Método para eliminar una categoría por ID
    }
}
