using RetailApp.Backend.Models;
using System.Linq;

namespace RetailApp.Backend.Interfaces
{
    public interface IBrandService // Interfaz para el servicio de Marcas
    {
        IQueryable<Brand> GetAllBrands();// Método para obtener todas las marcas
        Task<Brand?> GetBrandByIdAsync(int id); // Método para obtener una marca por ID
        Task<Brand> CreateBrandAsync(Brand brand); // Método para crear una nueva marca
        Task<bool> UpdateBrandAsync(Brand brand); // Método para actualizar una marca existente
        Task<bool> DeleteBrandAsync(int id); // Método para eliminar una marca por ID
    }
}
