using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MedicinaESE.Services;

namespace MedicinaESE.Pages;

public class LoginModel : PageModel
{
    // Propiedades que reciben los datos del formulario
    [BindProperty] public string Documento { get; set; } = "";
    [BindProperty] public string Contraseña { get; set; } = "";
    
    // Servicio de autenticación para manejar el acceso a la base de datos
    private readonly AuthService _authService;

    public LoginModel(AuthService authService)
    {
        _authService = authService;
    }

    public IActionResult OnPost()
    {
        // Obtener el número de intentos fallidos desde la sesión
        int intentosActuales = HttpContext.Session.GetInt32("IntentosFallidos") ?? 0;

        // Si el usuario ha fallado 10 veces, bloquear el acceso
        if (intentosActuales >= 10)
        {
            TempData["Mensaje"] = "bloqueado";
            return Page();
        }

        // Validar credenciales con `AuthService`
        var resultado = _authService.ValidarCredenciales(Documento, Contraseña);

        if (resultado.Exitoso)
        {
            // Restablecer el contador de intentos fallidos en sesión
            HttpContext.Session.SetInt32("IntentosFallidos", 0);

            // Guardar información del usuario en sesión
            HttpContext.Session.SetString("UsuarioDocumento", Documento);
            HttpContext.Session.SetString("UsuarioNombre", resultado.Nombre);
            HttpContext.Session.SetString("UsuarioCorreo", resultado.Correo);
            HttpContext.Session.SetString("UsuarioTipo", resultado.TipoUsuario);

            // Definir mensaje de éxito y redirección
            TempData["Mensaje"] = "success";
            TempData["Redireccion"] = resultado.TipoUsuario switch
            {
                "admin" => "/admin-main",
                "medico" => "/ficha-medica",
                "paciente" => "/agendar-cita",
                _ => "/ErrorBD"
            };
        }
        else
        {
            // Incrementar intentos fallidos si el acceso falla
            intentosActuales++;
            HttpContext.Session.SetInt32("IntentosFallidos", intentosActuales);
            
            // Guardar intentos restantes
            TempData["IntentosRestantes"] = 10 - intentosActuales;
            TempData["Mensaje"] = resultado.Mensaje;
        }

        return Page();
    }
}
