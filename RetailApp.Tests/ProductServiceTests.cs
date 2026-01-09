using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data; // Asegúrate de que esta sea la ruta de tu DbContext
using RetailApp.Backend.Models;
using RetailApp.Backend.Services;
using Xunit;

namespace RetailApp.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetProductById_DeberiaRetornarProducto_DesdeBaseDatosEnMemoria()
        {
            // 1. ARRANGE: Configuramos una base de datos en memoria (Fake)
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "RetailApp_Test_Db")
                .Options;

            // Insertamos un dato real en esa base de datos falsa
            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Zapatillas Zafra", BrandId = 1, CategoryId = 1 });
                await context.SaveChangesAsync();
            }

            // 2. ACT: Ejecutamos el servicio usando ese contexto en memoria
            using (var context = new ApplicationDbContext(options))
            {
                var servicio = new ProductService(context);
                var resultado = await servicio.GetProductByIdAsync(1);

                // 3. ASSERT: Verificamos
                Assert.NotNull(resultado);
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
    }
}