using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class Inventory // Clase Inventory (Inventario)
    {
        [Key] // Atributo que indica que esta propiedad es la clave primaria
        public int Id { get; set; } // Identificador único del inventario
        public int ProductVariantId { get; set; } // Identificador de la variante del producto

        [Required] // Atributo que indica que esta propiedad es obligatoria
        public int Quantity { get; set; } // Cantidad disponible en inventario
        public DateTime LastUpdated { get; set; } // Fecha de la última actualización del inventario

        //Propiedad de navegación para la relación con ProductVariant
        public ProductVariant ProductVariant { get; set; } // Relación con la entidad ProductVariant

    }
}
