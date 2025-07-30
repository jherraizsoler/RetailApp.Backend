using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        // Propiedad de navegación para la relación N:N con Product
        public ICollection<StoreProduct>? StoreProducts { get; set; }


    }
}
