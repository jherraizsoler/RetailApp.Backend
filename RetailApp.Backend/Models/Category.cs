using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class Category // Clase Categoría (Categoría)
    {
        [Key] // Clave primaria (Primary Key)
        public int Id { get; set; } // Identificador único de la categoría (Unique identifier for the category)
        [Required] // Campo requerido (Required field)
        [StringLength(100)] // Longitud máxima de 100 caracteres (Maximum length of 100 characters)
        public string Name { get; set; } // Nombre de la categoría (Name of the category)

        [StringLength(500)] // Longitud máxima de 500 caracteres (Maximum length of 500 characters)
        public string? Description { get; set; } // Descripción de la categoría (Description of the category)


        public int? ParentCategoryId { get; set; } // Identificador de la categoría padre (Identifier of the parent category)

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Fecha de creación (Creation date)


        // Propiedades de navegación (Navigation properties)
        public Category? ParentCategory { get; set; } // Categoría padre (Parent category)
        public ICollection<Category>? ChildCategories { get; set; } // Subcategorías

        public ICollection<Product>? Products { get; set; } // Colección de productos asociados a la categoría (Collection of products associated with the category)

    }
}
