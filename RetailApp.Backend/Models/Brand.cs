using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace RetailApp.Backend.Models
{
    public class Brand // Clase Brand (Marca)
    {
        [Key] // Marca Id como Clave Primaria
        public int Id { get; set; }

        [Required] // Marca Name como Requerido
        [StringLength(100)] // Limita la longitud de Name a 100 caracteres
        
        public string Name { get; set; } = string.Empty; // Nombre de la marca
        
        
        [StringLength(500)] // Limita la longitud de Description a 255 caracteres
        public string? Description { get; set; } // Descripción de la marca (opcional)

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Fecha de creación

        // Propiedad de navegación inversa a Product
        public ICollection<Product>? Products { get; set; }
    }
}
