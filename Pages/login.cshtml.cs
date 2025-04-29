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

        public void OnPost()
        {
            if (Documento == "admin" && Contraseña == "1234")
            {
                Mensaje = "success"; // SweetAlert mostrará "acceso permitido"
            }
            else
            {
                Mensaje = "error"; // SweetAlert mostrará "acceso no permitido"
            }
        }
    }
}
