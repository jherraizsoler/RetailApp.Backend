using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class Role // Clase Role (Rol)
    {
        [Key] // Marca Id como Clave Primaria
        public int Id { get; set; }

        [Required] // Marca Name como Requerido
        [StringLength(50)] // Limita la longitud de Name a 50 caracteres
        public string Name { get; set; } = string.Empty; // Nombre del rol

        // Propiedad de navegación inversa
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
