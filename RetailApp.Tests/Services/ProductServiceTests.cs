using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data; // Asegúrate de que esta sea la ruta de tu DbContext
using RetailApp.Backend.Models;
using RetailApp.Backend.Services;
using Xunit;

namespace RetailApp.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetProductById_DeberiaRetornarProducto_DesdeBaseDatosEnMemoria()
        {
            // 1. ARRANGE: Usamos Guid.NewGuid() para que la base de datos sea 100% nueva y limpia
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // Asegúrate de añadir la Brand y Category si son obligatorias en tu modelo
                var brand = new Brand { Id = 1, Name = "Marca Test" };
                var category = new Category { Id = 1, Name = "Categoria Test" };
                context.Brands.Add(brand);
                context.Categories.Add(category);

                context.Products.Add(new Product
                {
                    Id = 1,
                    Name = "Zapatillas Zafra",
                    BrandId = 1,
                    CategoryId = 1,
                    URL_Slug = "zapatillas-test-" + Guid.NewGuid().ToString() // Slug único
                });
                await context.SaveChangesAsync();
            }

            // 2. ACT
            using (var context = new ApplicationDbContext(options))
            {
                var servicio = new ProductService(context);
                var resultado = await servicio.GetProductByIdAsync(1);

                // 3. ASSERT
                Assert.NotNull(resultado); // Aquí es donde fallaba
                Assert.Equal("Zapatillas Zafra", resultado.Name);
            }
        }
        [Fact]
        public async Task GetProductById_DeberiaRetornarNull_SiElProductoNoExiste()
        {
            // 1. ARRANGE: Base de datos vacía
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var servicio = new ProductService(context);

                // 2. ACT: Buscamos el ID 999 que no existe
                var resultado = await servicio.GetProductByIdAsync(999);

                // 3. ASSERT: Verificamos que sea null
                Assert.Null(resultado);
            }
        }
        [Fact]
        public async Task CreateProductAsync_DeberiaGuardarProductoCorrectamente()
        {
            // 1. ARRANGE: Configuramos el contexto y el producto a crear
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var nuevoProducto = new Product
            {
                Id = 10,
                Name = "Camiseta Neoris Especial",
                BrandId = 1,
                CategoryId = 1,
                URL_Slug = "camiseta-neoris-especial" // Recordando que en tu DbContext este campo es único
            };

            // 2. ACT: Llamamos al servicio para guardar
            using (var context = new ApplicationDbContext(options))
            {
                var servicio = new ProductService(context);
                await servicio.CreateProductAsync(nuevoProducto);
            }

            // 3. ASSERT: Verificamos que el producto existe en la DB
            using (var context = new ApplicationDbContext(options))
            {
                var productoEnDb = await context.Products.FindAsync(10);

                Assert.NotNull(productoEnDb);
                Assert.Equal("Camiseta Neoris Especial", productoEnDb.Name);
                Assert.Equal(1, await context.Products.CountAsync());
            }
        }
    }
}