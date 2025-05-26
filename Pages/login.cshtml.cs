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
        public string Redireccion { get; set; } = "";

        public IActionResult OnPost()
        {
            if (Documento == "admin" && Contraseña == "1234")
            {
                Mensaje = "success"; // SweetAlert mostrará "acceso permitido"
                Redireccion = "/admin-main"; // Redirige al panel de administración
            }
            else if (Documento == "paciente" && Contraseña == "1234")
            {
                Mensaje = "success"; // SweetAlert mostrará "acceso permitido"
                Redireccion = "/agendar-cita"; // Redirige a agendar cita
            }
            else if (Documento == "medico" && Contraseña == "1234")
            {
                Mensaje = "success"; // SweetAlert mostrará "acceso permitido"
                Redireccion = "/ficha-medica"; // Redirige a ficha médica
            }
            else
            {
                Mensaje = "error"; // SweetAlert mostrará "acceso no permitido"
            }
            
            return Page(); // Permite mostrar la alerta antes de redirigir
        }
    }
}
