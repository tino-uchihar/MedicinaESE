using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicinaESE.Pages;

public class PerfilUsuarioModel : PageModel
{
    public string DocumentoId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string TipoUsuario { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UsuarioDocumento") == null)
        {
            //return RedirectToPage("/Comun/ErrorBD"); // Si no hay sesión, redirigir a la página de error
        }

        DocumentoId = HttpContext.Session.GetString("UsuarioDocumento") ?? "No disponible";
        Nombre = HttpContext.Session.GetString("UsuarioNombre") ?? "No disponible";
        Correo = HttpContext.Session.GetString("UsuarioCorreo") ?? "No disponible";
        TipoUsuario = HttpContext.Session.GetString("UsuarioTipo") ?? "No disponible";

        return Page();
    }
}
