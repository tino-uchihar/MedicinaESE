using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Medicinaese.Pages
{
    public class admin_mainModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                TempData["MensajeError"] = "Acceso denegado";
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
