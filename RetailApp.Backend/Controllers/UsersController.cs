using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailApp.Backend.Controllers
{

    [Route("api/[controller]")] // Define la ruta base para el controlador (ej. /api/Users)
    [ApiController] // Indica que este controlador es un API Controller
    public class UsersController : ControllerBase //Hereda de ControllerBase para ser un controlador de API
    {
        private readonly IUserService _userService; // Interfaz del servicio de usuario
        // Constructor que inyecta el servicio de usuario
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: /api/Users
        //      Método para obtener todos los usuarios
        [HttpGet] // Define el método HTTP GET para esta acción
        public async Task<ActionResult> GetUsers([FromQuery] string? search)
        {
            // 1. Iniciamos consulta diferida (IQueryable)
            var query = _userService.GetAllUsers();

            // 2. Filtros dinámicos usando tus campos: Email, FirstName, LastName
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Email.Contains(search) ||
                                         u.FirstName.Contains(search) ||
                                         u.LastName.Contains(search));
            }

            // 3. PROYECCIÓN DE SEGURIDAD (Data Shaping)
            // Seleccionamos campos específicos. PasswordHash se queda en la base de datos.
            var users = await query
                .Select(u => new {
                    u.Id,
                    u.Email,
                    FullName = u.FirstName + " " + u.LastName, // Concatenación en el servidor
                    u.PhoneNumber,
                    u.RegistrationDate,
                    u.IsActive,
                    // Si necesitas contar roles:
                    RolesCount = u.UserRoles != null ? u.UserRoles.Count : 0
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: /api/Users/5
        [HttpGet("{id}")] // Define el método HTTP GET para obtener un usuario por ID
        public async Task<ActionResult> GetUser(int id)
        {
            // Buscamos y proyectamos en una sola operación
            var user = await _userService.GetAllUsers()
                .Select(u => new {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.PhoneNumber,
                    u.RegistrationDate,
                    u.LastLoginDate,
                    u.IsActive
                    // PasswordHash queda EXCLUIDO aquí también
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            return Ok(user);
        }

        // POST: /api/Users
        // Para protegerse de ataques de overposting, consulta https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] // Define el método HTTP POST para crear un nuevo usuari
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Crea un nuevo usuario usando el servicio
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser); // Devuelve 201 Created con la ubicación del nuevo usuario
        }

        // PUT: /api/Users/5
        // Para protegerse de ataques de overposting, consulta https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] // Define el método HTTP PUT para actualizar un usuario por ID
        public async Task<IActionResult> PutUser(int id, User user)
        {
            // Verifica si el ID del usuario coincide con el ID de la URL
            if (id != user.Id)
            {
                return BadRequest(); // Devuelve 400 Bad Request si el ID no coincide
            }

            // Intenta actualizar el usuario usando el servicio
            var success = await _userService.UpdateUserAsync(user);
            if (!success)
            {
                return NotFound(); // Devuelve 404 Not Found si el usuario no existe
            }
            return NoContent(); // Devuelve 204 No Content si la actualización fue exitosa
        }


        // DELETE: /api/Users/5
        [HttpDelete("{id}")] // Define el método HTTP DELETE para eliminar un usuario por ID
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Intenta eliminar el usuario usando el servicio
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound(); // Devuelve 404 Not Found si el usuario no existe
            }
            return NoContent(); // Devuelve 204 No Content si la eliminación fue exitosa
        }




        // Aquí puedes añadir más métodos para manejar otras operaciones (crear, actualizar, eliminar usuarios, etc.)
    }
}
