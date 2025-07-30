using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class User // Clase User (Usuario)
    {
        [Key] // Marca Id como Clave Primaria
        public int Id { get; set; }

        [Required] // Marca Name como Requerido
        [StringLength(255)] // Limita la longitud de email a 255 caracteres
        public string Email { get; set; } = string.Empty;

        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty; // Nombre

        [StringLength(100)]
        public string LastName { get; set; } = string.Empty; // Apellido

        [StringLength(20)]
        public string? PhoneNumber { get; set; } // Número de Teléfono (puede ser nulo)


        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow; // Fecha de Registro (UTC)
        public DateTime? LastLoginDate { get; set; } // Última fecha de login (puede ser nula)


        // Propiedad IsActive indica si el usuario está activo
        public bool IsActive { get; set; } = true;  // Indica si el usuario está activo

        // Propiedad de navegación para los roles del usuario
        public ICollection<UserRole>? UserRoles { get; set; }

    }
}
