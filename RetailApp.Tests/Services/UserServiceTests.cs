using Microsoft.EntityFrameworkCore;
using RetailApp.Backend.Data;
using RetailApp.Backend.Models;
using RetailApp.Backend.Services;
using Xunit;

namespace RetailApp.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserByIdAsync_DeberiaRetornarUsuario_CuandoIdExiste()
        {
            // 1. ARRANGE: Configuramos la DB en memoria limpia
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UserTest_" + Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Email = "jorge@zafra.com",
                    FirstName = "Jorge",
                    LastName = "Zafra",
                    PasswordHash = "hashed_123"
                });
                await context.SaveChangesAsync();
            }

            // 2. ACT: Usamos GetUserByIdAsync que es el método real de tu servicio
            using (var context = new ApplicationDbContext(options))
            {
                var servicio = new UserService(context);
                var resultado = await servicio.GetUserByIdAsync(1);

                // 3. ASSERT: Verificamos los resultados
                Assert.NotNull(resultado);
                Assert.Equal("Jorge", resultado.FirstName);
                Assert.Equal(1, resultado.Id);
            }
        }

        [Fact]
        public async Task DeleteUserAsync_DeberiaRetornarTrue_SiElUsuarioSeElimina()
        {
            // ARRANGE
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteUserTest_" + Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Users.Add(new User { Id = 5, Email = "test@delete.com", FirstName = "Test", LastName = "User" });
                await context.SaveChangesAsync();
            }

            // ACT
            using (var context = new ApplicationDbContext(options))
            {
                var servicio = new UserService(context);
                var resultado = await servicio.DeleteUserAsync(5);

                // ASSERT
                Assert.True(resultado);
                Assert.Empty(context.Users); // Verificamos que la lista esté vacía
            }
        }
    }
}
