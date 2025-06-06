using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MedicinaESE.Pages.Admin
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Cerrar autenticación basada en cookies (elimina el usuario autenticado)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpiar la sesión para eliminar cualquier otro dato almacenado
            HttpContext.Session.Clear();

            // Redirigir al usuario al login después de cerrar sesión
            return RedirectToPage("/Login");
        }
    }
}
