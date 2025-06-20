using MedicinaESE.Services;
using Microsoft.EntityFrameworkCore;
using MedicinaESE.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DoctorNowDB")
                       ?? throw new InvalidOperationException("La cadena de conexión no está definida en appsettings.json");

// Registrar EF Core con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// Registrar Razor Pages para que la aplicación funcione con páginas dinámicas
builder.Services.AddRazorPages();

// Registrar controladores para los endpoints API
builder.Services.AddControllers();

// ----------------------
// REGISTRO DE POLÍTICAS Y CONVENCIAS PARA AUTORIZACIÓN
// ----------------------
// Establece convenciones para que las carpetas se protejan de forma centralizada:
builder.Services.Configure<RazorPagesOptions>(options =>
{
    // Solo los usuarios con el claim UsuarioTipo = "admin" podrán acceder a /Admin
    options.Conventions.AuthorizeFolder("/Admin", "RequireAdmin");
    // Para /Medico se permite a usuarios con UsuarioTipo "medico" o "admin"
    options.Conventions.AuthorizeFolder("/Medico", "RequireMedico");
    // Para /Paciente se permite a usuarios con UsuarioTipo "paciente", "medico" o "admin"
    options.Conventions.AuthorizeFolder("/Paciente", "RequirePaciente");
});

// Registrar las políticas de autorización basadas en "UsuarioTipo"
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
    {
        policy.RequireClaim("UsuarioTipo", "admin");
    });
    options.AddPolicy("RequireMedico", policy =>
    {
        policy.RequireClaim("UsuarioTipo", "medico", "admin");
    });
    options.AddPolicy("RequirePaciente", policy =>
    {
        policy.RequireClaim("UsuarioTipo", "paciente", "medico", "admin");
    });
});
// ----------------------
// FIN DEL BLOQUE DE POLÍTICAS Y CONVENCIAS
// ----------------------

// Registrar el servicio de Noticias
builder.Services.AddScoped<NoticiaService>();

// Registrar el servicio de Autenticación
builder.Services.AddScoped<AuthService>();

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

// Redireccionar errores 404 a /Comun/Error404
app.UseStatusCodePagesWithRedirects("/Comun/Error404");

// Mapear los controladores creados en la carpeta Controllers
app.MapControllers();

// Mapear Razor Pages
app.MapRazorPages();

app.Run();
