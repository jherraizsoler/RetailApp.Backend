using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailApp.Backend.Models
{
    public class Product // Clase Producto (Product)
    {
        [Key] // Clave primaria (Primary Key)
        public int Id { get; set; } // Identificador único del producto (Unique identifier for the product)

        [Required] // Campo requerido (Required field)
        [StringLength(255)] // Longitud máxima de 255 caracteres (Maximum length of 255 characters)
        public string Name { get; set; } // Nombre del producto (Name of the product)

        [StringLength(500)]
        public string? ShortDescription { get; set; } // Descripción corta del producto (Short description of the product)

        public string? LongDescription { get; set; }  // Descripción larga (MAX en SQL Server)


        public int BrandId { get; set; } // FK a Brand (FK to Brand)
        public int CategoryId { get; set; } // FK a Category (FK to Category)
        public int? CollectionId { get; set; } // FK a Collection (FK to Collection)

        [StringLength(100)] // Longitud máxima de 100 caracteres (Maximum length of 100 characters)
        public string? Manufacturer { get; set; } // Fabricante del producto (Manufacturer of the product)

        [StringLength(50)] // Longitud máxima de 50 caracteres (Maximum length of 50 characters)
        public string? ModelNumber { get; set; } // Número de modelo del producto (Model number of the product)


        [Column(TypeName = "decimal(10,2)")]  // Tipo de dato decimal con 10 dígitos en total y 2 decimales (Decimal type with 10 digits in total and 2 decimals)
        public decimal BasePrice { get; set; } // Precio base del producto (Base Price of the product)

        public bool IsActive { get; set; } = true; // Indica si el producto está activo (Indicates if the product is active)


        public DateTime? AvailableFromDate { get; set; } // Fecha desde la cual el producto está disponible (Date from which the product is available)
        public DateTime? AvailableToDate { get; set; } // Fecha hasta la cual el producto está disponible (Date until which the product is available)


        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Fecha de creación (Creation date)
        public DateTime? LastModifiedDate { get; set; } // Fecha de última modificación (Last modified date)

        [StringLength(255)] // Longitud máxima de 255 caracteres (Maximum length of 255 characters)
        public string? SEO_Title { get; set; } // Título SEO del producto (SEO title of the product)
        [StringLength(500)] // Longitud máxima de 500 caracteres (Maximum length of 500 characters)
        public string? SEO_MetaDescription { get; set; } // Descripción SEO del producto (SEO meta description of the product)


        [StringLength(255)] // Longitud máxima de 255 caracteres (Maximum length of 255 characters)
        public string? URL_Slug { get; set; } // URL amigable del producto (Friendly URL of the product)

        public string? Keywords { get; set; } // Palabras clave para SEO (Keywords for SEO)

        public bool IsFeatured { get; set; } = false; // Indica si el producto es destacado (Indicates if the product is featured)
        public bool IsNewArrival { get; set; } = false; // Indica si el producto es una novedad (Indicates if the product is a new arrival)

        [StringLength(int.MaxValue)] // Longitud máxima de int.MaxValue caracteres (Maximum length of int.MaxValue characters)
        public string? MainImageUrl { get; set; } // URL de la imagen principal del producto (URL of the main product image)
        public string? ThumbnailUrl { get; set; } // URL de la miniatura del producto (URL of the product thumbnail)
        public string? VideoUrl { get; set; } // URL del video del producto (URL of the product video)




        // Propiedades de navegación (Navigation properties)
        public Brand? Brand { get; set; } // Marca del producto (Brand of the product)
        public Category? Category { get; set; } // Categoría del producto (Category of the product)
        public Collection? Collection { get; set; } // Colección del producto (Collection of the product)
        public ICollection<ProductVariant>? Variants { get; set; } // Variantes del producto (Variants of the product)
        public ICollection<ProductImage>? ProductImages { get; set; } // Imágenes del producto (Product images)
        public ICollection<StoreProduct>? StoreProducts { get; set; } // Productos disponibles en tiendas (Products available in stores)
        public ICollection<WarehouseProduct>? WarehouseProducts { get; set; } // Productos disponibles en almacenes (Products available in warehouses)

    }
}
