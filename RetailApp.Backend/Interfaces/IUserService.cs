using RetailApp.Backend.Models;
using System.Linq;

namespace RetailApp.Backend.Interfaces
{
    public interface IUserService // Interfaz para el servicio de Usuarios
    {
        IQueryable<User> GetAllUsers(); // Método para obtener todos los usuarios filtrados en DB
        Task<User?> GetUserByIdAsync(int id); // Método para obtener un usuario por ID
        Task<User> CreateUserAsync(User user); // Método para crear un nuevo usuario
        Task<bool> UpdateUserAsync(User user); // Método para actualizar un usuario existente
        Task<bool> DeleteUserAsync(int id); // Método para eliminar un usuario por ID

    }
}
