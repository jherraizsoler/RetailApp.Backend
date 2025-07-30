using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Linq; // Agrega esto para usar .Any()
using System.Threading.Tasks;

namespace RetailApp.Backend.Services
{
    public class ProductService : IProductService // Implement IProductService interface
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            // Obtiene todos los productos, incluyendo su Brand y Category
            return await _context.Products
                .Include(p => p.Brand) // Incluye la marca del producto
                .Include(p => p.Category) // Incluye la categoría del producto
                .ToListAsync();

        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            // Obtener un producto por su ID, incluyendo su Brand, Category y sus variantes
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Variants) // Incluye las variantes del producto
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            // Añadir un nuevo producto a la base de datos
            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return product; // Retorna el producto creado (Con su ID generado)
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            // Marcar el producto como modificado
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true; // Retorna true si la actualización fue exitosa
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejar si el producto no existe para actualizar
                if (!_context.Products.Any(e => e.Id == product.Id))
                {
                    return false; // Retorna false si el producto no existe
                }
                else
                {
                    throw; // Lanza la excepción si es otro tipo de error
                }
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            // Buscar el producto por su ID
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Retorna false si el producto no existe
            }
            // Eliminar el producto de la base de datos
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return true; // Retorna true si la eliminación fue exitosa
        }
    }
}
