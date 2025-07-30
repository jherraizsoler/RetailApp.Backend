using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class Collection // Clase Collection (Colección de Productos)
    {
        [Key] // Clave primaria (Primary Key)
        public int Id { get; set; } // Identificador único de la colección (Unique identifier for the collection)

        [Required] // Campo requerido (Required field)
        [StringLength(100)] // Longitud máxima de 100 caracteres (Maximum length of 100 characters)
        public string Name { get; set; } = string.Empty; // Nombre de la colección (Name of the collection)

        [StringLength(500)] // Longitud máxima de 500 caracteres (Maximum length of 500 characters)
        public string? Description { get; set; } // Descripción de la colección (Description of the collection)

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Fecha de creación de la colección (Creation date of the collection)


        // Propiedades de navegación (Navigation properties)
        public ICollection<Product>? Products { get; set; } // Colección de productos asociados a la colección (Collection of products associated with the collection)
    }
}
