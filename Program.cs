using MedicinaESE.Services;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
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

// Registrar el servicio de Noticias
builder.Services.AddSingleton<NoticiaService>(provider => new NoticiaService(connectionString));

// Registrar el servicio de Autenticación
builder.Services.AddSingleton<AuthService>();

// Registrar IHttpContextAccessor para poder acceder a HttpContext (opcional en vistas)
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Configurar la autenticación: Usaremos **cookies con claims** para la identidad en la web 
// y también configuramos JWT para proteger endpoints de APIs.
builder.Services.AddAuthentication(options =>
{
    // Usaremos el esquema de autenticación de cookies como por defecto
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Configuración de la cookie de autenticación
    options.LoginPath = "/login"; // Ruta de login
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tiempo de expiración
    options.Cookie.HttpOnly = true; // Marca la cookie como inaccesible vía JavaScript
    options.Cookie.IsEssential = true; // Se considera esencial para la aplicación
})
.AddJwtBearer(options =>
{
    // Configuración JWT para endpoints de API, por ejemplo
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

// Habilitar sesiones para almacenar información temporal (como intento de login)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Expira tras 30 minutos de inactividad
    options.Cookie.HttpOnly = true; // Protege contra scripts maliciosos
    options.Cookie.IsEssential = true; // Asegura que la sesión siempre esté activa
});

var app = builder.Build();

// Configurar el pipeline de ejecución
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Página de error en producción
    app.UseHsts(); // Aumenta la seguridad HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Activar la autenticación (cookies y JWT)
app.UseAuthentication();

// Habilitar sesiones
app.UseSession();

// Activar autorización
app.UseAuthorization();

// Redireccionar errores 404 a /Error404
app.UseStatusCodePagesWithRedirects("/Error404");

// Mapear Razor Pages
app.MapRazorPages();

app.Run();
