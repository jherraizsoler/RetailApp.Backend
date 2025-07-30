using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class ProductImage // Clase Producto Imagen (ProductImage)
    {
        [Key]
        public int Id { get; set; } // Unique identifier for the product image
        
        public int ProductId { get; set; } // Foreign key to the associated product

        [Required]
        [StringLength(int.MaxValue)] // Maximum length for the image URL, allowing for very long URLs
        public string ImageUrl { get; set; } = string.Empty; // URL of the product image
       
        public string? AltText { get; set; } // Alternative text for the image, useful for accessibility

        public int DisplayOrder { get; set; } = 0; // Order in which the image should be displayed, default is 0

        public bool IsMain { get; set; } //Indica si es la imagen principal del producto (Indicates if this is the main image of the product)

        public DateTime UploadDate { get; set; } = DateTime.UtcNow; // Date when the image was uploaded, default is current UTC time

        public Product Product { get; set; } = default!; // Navigation property to the associated product
    }
}
