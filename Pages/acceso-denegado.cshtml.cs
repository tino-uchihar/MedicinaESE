using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicinaESE.Pages
{
    public class AccesoDenegadoModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Configuramos TempData para mostrar "Acceso denegado" en el Index
            TempData["MensajeError"] = "Acceso denegado";
            return RedirectToPage("/Index");
        }
    }
}
