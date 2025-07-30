using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailApp.Backend.Models
{
    public class OrderItem // Clase OrderItem (Artículo de Pedido / Order Item)
    {
        [Key] // Clave primaria (Primary Key)
        public int Id { get; set; } // Identificador único del artículo de pedido (Unique identifier for the order item)
        public int OrderId { get; set; } // Identificador del pedido al que pertenece el artículo (Identifier of the order to which the item belongs)
        public int ProductVariantId { get; set; } // Identificador de la variante del producto (Identifier of the product variant)
        [Required] // Requerido (Required)
        public int Quantity { get; set; } // Cantidad del artículo en el pedido (Quantity of the item in the order)
        [Required] // Requerido (Required)
        [Column(TypeName = "decimal(10,2)")] // Tipo de dato decimal con 10 dígitos en total y 2 decimales (Decimal type with 10 digits in total and 2 decimals)
        public decimal PriceAtPurchase { get; set; } // Precio del artículo en el momento de la compra (Price of the item at the time of purchase)

        // Propiedades de navegación
        public Order Order { get; set; } = default!; // Relación con el pedido (Relationship with the order)
        public ProductVariant ProductVariant { get; set; } = default!; // Relación con la variante del producto (Relationship with the product variant)
    }
}
