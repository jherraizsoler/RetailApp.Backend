using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailApp.Backend.Models
{
    public class WarehouseProduct
    {
        [Key]
        [Column(Order = 1)] // Parte de la clave primaria compuesta
        public int WarehouseId { get; set; }
        [Key]
        [Column(Order = 2)] // Parte de la clave primaria compuesta
        public int ProductId { get; set; }


        [Required]
        public int StockQuantity { get; set; } // Cantidad de stock del producto en el almacén  (Crucial para el inventario)

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow; // Fecha y hora de la última actualización del stock

        // Propiedad de navegación hacia Warehouse
        public Warehouse Warehouse { get; set; } = default!; // Almacen asociado
        public Product Product { get; set; } = default!; // Producto asociado

    }
}
