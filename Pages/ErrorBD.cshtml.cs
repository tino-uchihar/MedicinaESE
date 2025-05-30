using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicinaESE.Pages;

public class ErrorBDModel : PageModel
{
    public void OnGet()
    {
        Response.StatusCode = 503; // Indica "Service Unavailable" en la respuesta HTTP
    }
}
