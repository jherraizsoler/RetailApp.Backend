namespace RetailApp.Backend.Interfaces
{
    public interface IUserService // Interfaz para el servicio de Usuarios
    {
        Task<IEnumerable<User>> GetAllUsersAsync(); // Método para obtener todos los usuarios
        Task<User?> GetUserByIdAsync(int id); // Método para obtener un usuario por ID
        Task<User> CreateUserAsync(User user); // Método para crear un nuevo usuario
        Task<bool> UpdateUserAsync(User user); // Método para actualizar un usuario existente
        Task<bool> DeleteUserAsync(int id); // Método para eliminar un usuario por ID

    }
}
