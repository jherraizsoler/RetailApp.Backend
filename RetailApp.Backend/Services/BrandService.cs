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
        public IQueryable<Brand> GetAllBrands()
        {
            // Retornamos la consulta lista para ser filtrada fuera
            return _context.Brands.AsNoTracking();
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
            // En lugar de buscar el objeto completo, creamos una instancia mínima con el ID
            var brand = new Brand { Id = id };

            // Le decimos a Entity Framework que empiece a trackear este objeto 
            // y luego lo marcamos para eliminar.
            _context.Entry(brand).State = EntityState.Deleted;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si el ID no existe en la DB, fallará al intentar borrarlo
                return false;
            }
        }
    }
}
