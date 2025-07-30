using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailApp.Backend.Models
{
    public class UserRole // Clase UserRole (Rol de Usuario)
    {

        [Key]
        [Column(Order = 1)] // Parte de la clave primaria compuesta
        public int UserId { get; set; } // Clave foránea a User

        [Key]
        [Column(Order = 2)] // Parte de la clave primaria compuesta
        public int RoleId { get; set; } // Clave foránea a Role




        // Propiedad de navegación
        public User User { get; set; } = default!; // Usuario asociado

        public Role Role { get; set; } = default!; // Rol asociado

    }
}
