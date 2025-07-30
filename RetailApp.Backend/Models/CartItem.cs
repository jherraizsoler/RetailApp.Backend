using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class CartItem // Clase CartItem (Item del Carrito)
    {
        [Key] // Atributo Key para identificar el item del carrito
        public int Id { get; set; } // Identificador del item del carrito

        public int CartId { get; set; } // Identificador del carrito al que pertenece el item
        public int ProductVariantId { get; set; } // Identificador del producto

        [Required] // Atributo Required para asegurar que el campo no sea nulo
        public int Quantity { get; set; } // Cantidad del producto en el carrito

        public DateTime AddedDate { get; set; } // Fecha en que se agregó el item al carrito

        // Propiedades de navegación para establecer relaciones con otras entidades
        public Cart Cart { get; set; } = default!; // Relación con el carrito
        public ProductVariant ProductVariant { get; set; } = default!; // Relación con la variante del producto
    }
}
