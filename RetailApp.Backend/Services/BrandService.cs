using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Interfaces;
using RetailApp.Backend.Models;
using System.Collections.Generic;
using System.Linq; // Agrega esto para usar .Any()
using System.Threading.Tasks;

namespace RetailApp.Backend.Services
{
    public class BrandService : IBrandService // Implement IBrandService interface
    {
        private readonly ApplicationDbContext _context;
        public BrandService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            // Obtiene todas las marcas
            return await _context.Brands.ToListAsync();
        }
        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            // Obtener una marca por su ID
            return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Brand> CreateBrandAsync(Brand brand)
        {
            // Añadir una nueva marca a la base de datos
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return brand; // Retorna la marca creada (Con su ID generado)
        }
        public async Task<bool> UpdateBrandAsync(Brand brand)
        {
            // Marcar la marca como modificada
            _context.Entry(brand).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true; // Retorna true si la actualización fue exitosa
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejar si la marca no existe para actualizar
                if (!_context.Brands.Any(e => e.Id == brand.Id))
                {
                    return false; // Retorna false si la marca no existe
                }
                throw; // Vuelve a lanzar la excepción si es otro tipo de error
            }
        }
        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await GetBrandByIdAsync(id);
            if (brand == null) return false; // Si no se encuentra la marca, retorna false
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return true; // Retorna true si la eliminación fue exitosa
        }
    }
}
