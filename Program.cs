using MedicinaESE.Services;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde appsettings.json
// Si no está definida, lanza un error para evitar fallos silenciosos
var connectionString = builder.Configuration.GetConnectionString("DoctorNowDB") 
                       ?? throw new InvalidOperationException("La cadena de conexión no está definida en appsettings.json");

// Registrar la conexión SQL para que esté disponible en la aplicación
builder.Services.AddSingleton(_ => new SqlConnection(connectionString));

// Registrar Razor Pages para que la aplicación funcione con páginas dinámicas
builder.Services.AddRazorPages();

// Registrar servicio de Noticias con la cadena de conexión
// Se usa `provider => new NoticiaService(connectionString)` para inyectar la conexión
builder.Services.AddSingleton<NoticiaService>(provider => new NoticiaService(connectionString));

var app = builder.Build();

// Configurar el pipeline de ejecución
// Si no estamos en modo desarrollo, usamos un manejador de errores y seguridad extra
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Redirige a la página `/Error` en caso de excepción global
    app.UseHsts(); // Añade seguridad HTTPS si la aplicación está en producción
}

// Habilitar HTTPS y archivos estáticos (CSS, imágenes, scripts, etc.)
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configurar el sistema de rutas para las páginas Razor
app.UseRouting();

// Agregar autorización si en el futuro se requiere autenticación de usuarios
app.UseAuthorization();

// Manejo de errores 404: Redirigir cualquier ruta no encontrada a `/Error404`
app.UseStatusCodePagesWithRedirects("/Error404");

// Mapeo de Razor Pages para que sean accesibles desde la web
app.MapRazorPages();

// Arrancar la aplicación
app.Run();
