using MedicinaESE.Services;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DoctorNowDB") 
                       ?? throw new InvalidOperationException("La cadena de conexión no está definida en appsettings.json");

// Registrar la conexión SQL para que esté disponible en la aplicación
builder.Services.AddSingleton(_ => new SqlConnection(connectionString));

// Registrar Razor Pages para que la aplicación funcione con páginas dinámicas
builder.Services.AddRazorPages();

// Registrar servicio de Noticias con la cadena de conexión
builder.Services.AddSingleton<NoticiaService>(provider => new NoticiaService(connectionString));

// Registrar servicio de Autentificacion
builder.Services.AddSingleton<AuthService>();


// Configurar autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "MedicinaESE",
            ValidAudience = "Usuarios",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ClaveSecretaSuperSegura"))
        };
    });

// Habilitar sesiones para almacenar información temporal del usuario
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Expira tras 30 minutos de inactividad
    options.Cookie.HttpOnly = true; // Protección contra accesos de scripts maliciosos
    options.Cookie.IsEssential = true; // Asegura que la sesión siempre esté activa
});

var app = builder.Build();

// Configurar el pipeline de ejecución
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

// Activar autenticación con JWT
app.UseAuthentication();

// Habilitar sesiones para gestionar usuarios activos
app.UseSession();

// Agregar autorización para futuras restricciones de acceso
app.UseAuthorization();

// Manejo de errores 404: Redirigir cualquier ruta no encontrada a `/Error404`
app.UseStatusCodePagesWithRedirects("/Error404");

// Mapeo de Razor Pages para que sean accesibles desde la web
app.MapRazorPages();

// Arrancar la aplicación
app.Run();
