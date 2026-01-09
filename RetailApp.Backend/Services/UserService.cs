
using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;

namespace RetailApp.Backend.Services
{
    public class UserService : IUserService // Implement IUserService interface

    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Obtiene todos los usuarios de la base de datos
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Añadir un nuevo usuario a la base de datos
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return user; // Retorna el usuario creado (Con su ID generado)
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Marcar el usuario como modificado y guardar los cambios
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true; // Retorna true si la actualización fue exitosa
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejar si el usuario no existe para actualizar
                if (!_context.Users.Any(e => e.Id == user.Id))
                {
                    return false; // Retorna false si el usuario no existe
                }
                else
                {
                    throw; // Lanza la excepción si es otro tipo de error
                }
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            // Buscar el usuario por ID
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false; // Retorna false si el usuario no existe
            }

            // Eliminar el usuario de la base de datos y guardar cambios
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true; // Retorna true si la eliminación fue exitosa
        }
    }
}
