using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailApp.Backend.Models
{
    public class ProductVariant // Clase Producto Variante (ProductVariant)
    {
        [Key] // Clave primaria (Primary Key)
        public int Id { get; set; } // Identificador único del producto Variante (Unique identifier for the product)

        [Required] // Campo requerido (Required field)
        [StringLength(50)] // Longitud máxima de 50 caracteres (Maximum length of 50 characters)
        public string SKUCode { get; set; } = string.Empty;  // Código SKU del producto variante (SKU code of the product variant)

        [StringLength(50)] // Longitud máxima de 50 caracteres (Maximum length of 50 characters)
        public string? Barcode { get; set; } // Código de barras del producto variante (Barcode of the product variant)

        [StringLength(50)] // Longitud máxima de 50 caracteres (Maximum length of 50 characters)
        public string? Color { get; set; } // Color del producto variante (Color of the product variant)

        [StringLength(50)] // Longitud máxima de 50 caracteres (Maximum length of 50 characters)
        public string? Size { get; set; } // Tamaño del producto variante (Size of the product variant)

        [StringLength(255)] // Longitud máxima de 255 caracteres (Maximum length of 255 characters)
        public string? MaterialComposition { get; set; } // Composición del material del producto variante (Material composition of the product variant)

        [Column(TypeName = "decimal(10,2)")] // Tipo de dato decimal con 10 dígitos en total y 2 decimales (Decimal type with 10 digits in total and 2 decimals)
        public decimal? Weight { get; set; } // Peso del producto variante (Weight of the product variant)

        [StringLength(100)] // Longitud máxima de 100 caracteres (Maximum length of 100 characters)
        public string? Dimensions { get; set; } // Dimensiones del producto variante (Dimensions of the product variant)

        public string? CareInstructions { get; set; } // Instrucciones de cuidado del producto variante (Care instructions for the product variant)

        [StringLength(20)] // Longitud máxima de 20 caracteres (Maximum length of 20 characters)
        public string? GenderTarget { get; set; } // Género al que está dirigido el producto

        [Column(TypeName = "decimal(10,2)")] // Tipo de dato decimal con 10 dígitos en total y 2 decimales (Decimal type with 10 digits in total and 2 decimals)
        public decimal? PriceAdjustment { get; set; } // Ajuste de precio del producto variante (Price adjustment of the product variant)

        [Column(TypeName = "decimal(10,2)")] // Tipo de dato decimal con 10 dígitos en total y 2 decimales (Decimal type with 10 digits in total and 2 decimals)
        public decimal? FinalPrice { get; set; } // Precio final del producto variante (Final price of the product variant)
        public bool IsActive { get; set; } = true; // Indica si la variante del producto está activa (Indicates if the product variant is active)

        [StringLength(int.MaxValue)]
        public string? VariantSpecificImageUrl { get; set; } // URL de la imagen específica de la variante del producto (URL of the variant-specific product image)

        // Propiedades de navegación (Navigation properties)
        public Product Product { get; set; } = null!; // Relación con el producto (Relationship with the product)
        public ICollection<Inventory>? Inventories { get; set; } // Colección de inventarios asociados a la variante del producto (Collection of inventories associated with the product variant)
        public ICollection<CartItem>? CartItems { get; set; } // Colección de artículos del carrito asociados a la variante del producto (Collection of cart items associated with the product variant)
        public ICollection<OrderItem>? OrderItems { get; set; } // Colección de artículos de pedido asociados a la variante del producto (Collection of order items associated with the product variant)






    }
}
