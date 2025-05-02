using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Medicinaese.Pages
{
    public class loginModel : PageModel
    {
        [BindProperty]
        public string Documento { get; set; } = "";

        [BindProperty]
        public string Contraseña { get; set; } = "";

        public string Mensaje { get; set; } = "";

        public IActionResult OnPost() //Cambio void OnPost() --> firma del método a IActionResult OnPost() que permite controlar la redirección
        {
            if (Documento == "admin" && Contraseña == "1234")
            {
                Mensaje = "success"; // SweetAlert mostrará "acceso permitido"
            }
            else
            {
                Mensaje = "error"; // SweetAlert mostrará "acceso no permitido"
            }
            
            return Page(); // Se queda en la misma página para permitir la alerta

        }
    }
}
