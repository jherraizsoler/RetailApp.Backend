using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailApp.Backend.Models
{
    public class Order // Clase Order (Pedido / Order)
    {
        [Key] // Clave primaria (Primary Key)
        public int Id { get; set; } // Identificador único del pedido (Unique identifier for the order)
        
        public int UserId { get; set; } // Identificador del usuario que realizó el pedido (Identifier of the user who placed the order)

        [Required] // Requerido (Required)
        [StringLength(50)] // Longitud máxima de 50 caracteres (Maximum length of 50 characters)
        public string OrderNumber { get; set; } = Guid.NewGuid().ToString(); // Número de pedido único (Unique order number)

        [Required] // Requerido (Required)
        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Fecha del pedido (Order date)


       [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; } = "Pending"; // Estado del pedido (ej. Pending, Processing, Shipped, Delivered, Cancelled)

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; } // Cantidad total del pedido

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ShippingCost { get; set; } // Costo de envío

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? DiscountAmount { get; set; } // Cantidad de descuento aplicada

        [StringLength(100)]
        public string? ShippingAddress { get; set; } // Dirección de envío

        [StringLength(100)]
        public string? BillingAddress { get; set; } // Dirección de facturación

        [StringLength(50)]
        public string? PaymentMethod { get; set; } // Método de pago

        [StringLength(50)]
        public string? TransactionId { get; set; } // ID de la transacción de pago

        public DateTime? ShippedDate { get; set; } // Fecha de envío
        public DateTime? DeliveredDate { get; set; } // Fecha de entrega

        // Propiedades de navegación
        public User User { get; set; } = default!; // Usuario asociado al pedido
        public ICollection<OrderItem>? OrderItems { get; set; } // Items del pedido
    }
}
