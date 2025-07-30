using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailApp.Backend.Models
{
    public class StoreProduct
    {

        [Key]
        [Column(Order = 1)] // Parte de la clave primaria compuesta
        public int StoreId { get; set; }

        [Key]
        [Column(Order = 2)] // Parte de la clave primaria compuesta
        public int ProductId { get; set; }


        // Atributos especificos de la relación (ej. Precio del producto en esta tienda, si es diferente al base)
        [Column(TypeName = "decimal(10,2)")]
        public decimal PriceStore { get; set; } // Precio del producto en esta tienda (Price of the product in this store)

        // Propiedad de navegación hacia Store
        public Store Store { get; set; } = default!; //Tienda asociada

        public Product Product { get; set; } = default!; // Producto asociado

    }
}
