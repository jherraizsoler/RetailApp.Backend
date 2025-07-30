using Microsoft.EntityFrameworkCore; // ¡Asegúrate de añadir esta línea!
using RetailApp.Backend.Data;       // ¡Asegúrate de añadir esta línea para tu DbContext!

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. (Añadir servicios al contenedor.)
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- INICIO DEL CÓDIGO A AÑADIR/VERIFICAR ---

// Obtiene la cadena de conexión llamada "DefaultConnection" de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra ApplicationDbContext en el contenedor de inyección de dependencias
// y lo configura para usar SQL Server con la cadena de conexión obtenida.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- FIN DEL CÓDIGO A AÑADIR/VERIFICAR ---


var app = builder.Build();

// Configure the HTTP request pipeline. (Configurar la canalización de solicitudes HTTP.)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();