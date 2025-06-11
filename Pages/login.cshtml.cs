using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MedicinaESE.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MedicinaESE.Pages
{
    public class LoginModel : PageModel
    {
        // Propiedades que reciben los datos del formulario de login
        [BindProperty]
        public string Documento { get; set; } = "";
        [BindProperty]
        public string Contraseña { get; set; } = ""; 

        // Servicio de autenticación para interactuar con la base de datos
        private readonly AuthService _authService;

        public LoginModel(AuthService authService)
        {
            _authService = authService;
        }

        // Modificamos el OnPost para que sea asíncrono y use la autenticación por cookies.
        public async Task<IActionResult> OnPostAsync()
        {
            // Obtener el número de intentos fallidos desde la sesión
            int intentosActuales = HttpContext.Session.GetInt32("IntentosFallidos") ?? 0;

            // Si el usuario ha fallado 10 veces, bloquear el acceso
            if (intentosActuales >= 10)
            {
                TempData["Mensaje"] = "bloqueado";
                return Page();
            }

            // Validar credenciales usando AuthService
            var resultado = _authService.ValidarCredenciales(Documento, Contraseña);

            if (resultado.Exitoso)
            {
                // Resetear el contador de intentos fallidos
                HttpContext.Session.SetInt32("IntentosFallidos", 0);

                // (Opcional) Puedes seguir guardando datos en sesión, si los necesitas
                HttpContext.Session.SetString("UsuarioDocumento", Documento);
                HttpContext.Session.SetString("UsuarioNombre", resultado.Nombre);
                HttpContext.Session.SetString("UsuarioCorreo", resultado.Correo);
                HttpContext.Session.SetString("UsuarioTipo", resultado.TipoUsuario);

                // Crear una lista de claims para el usuario
                var claims = new List<Claim>
                {
                    // El claim de nombre se usará para mostrar el nombre del usuario
                    new Claim(ClaimTypes.Name, resultado.Nombre),
                    // Un claim personalizado para el tipo de usuario, importante para redirecciones
                    new Claim("UsuarioTipo", resultado.TipoUsuario),
                    // También incluímos el correo
                    new Claim(ClaimTypes.Email, resultado.Correo),
                    // Claim para el documento we
                    new Claim("DocumentoId", Documento)
                };

                // Crear el ClaimsIdentity con el esquema de autenticación de cookies
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Definir las propiedades de la autenticación (por ejemplo, persistencia y expiración)
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                // Firmar la autenticación: Esto creará la cookie de autenticación firmada
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                // Definir mensaje de éxito y redirección según el tipo de usuario
                TempData["Mensaje"] = "success";
                TempData["Redireccion"] = resultado.TipoUsuario switch
                {
                    "admin" => "/admin-main",
                    "medico" => "/ficha-medica",
                    "paciente" => "/agendar-cita",
                    _ => "/Comun/ErrorBD"
                };
            }
            else
            {
                // Incrementar intentos fallidos y establecer mensaje de error
                intentosActuales++;
                HttpContext.Session.SetInt32("IntentosFallidos", intentosActuales);
                TempData["IntentosRestantes"] = 10 - intentosActuales;
                TempData["Mensaje"] = resultado.Mensaje;
            }

            return Page();
        }
    }
}
