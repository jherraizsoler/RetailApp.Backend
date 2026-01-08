using RetailApp.Backend.Models;

namespace RetailApp.Backend.Interfaces
{
    public interface ICategoryService // Interfaz para el servicio de Categorías
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(); // Método para obtener todas las categorías
        Task<Category?> GetCategoryByIdAsync(int id); // Método para obtener una categoría por ID
        Task<Category> CreateCategoryAsync(Category category); // Método para crear una nueva categoría
        Task<bool> UpdateCategoryAsync(Category category); // Método para actualizar una categoría existente
        Task<bool> DeleteCategoryAsync(int id); // Método para eliminar una categoría por ID
    }
}
