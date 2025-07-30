using System.ComponentModel.DataAnnotations;

namespace RetailApp.Backend.Models
{
    public class Cart // Clase Cart (Carrito de Compras)
    {
        [Key] // Atributo que indica que esta propiedad es la clave primaria
        public int Id { get; set; } // Identificador del carrito
        public int? UserId { get; set; } // Identificador del usuario propietario del carrito

        public DateTime CreatedDate { get; set; } // Fecha de creación del carrito
        public DateTime? LastUpdateDate { get; set; } // Fecha de la última actualización del carrito

        [StringLength(255)] // Atributo que limita la longitud de la cadena a 255 caracteres
        public string? SessionId { get; set; } // Identificador de sesión del carrito, opcional

        // Propiedad de navegación para la relación con los elementos del carrito
        public User? User { get; set; } // Usuario propietario del carrito
        public ICollection<CartItem>? CartItems { get; set; } // Colección de elementos del carrito


    }
}
