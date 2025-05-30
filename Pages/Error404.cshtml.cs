using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicinaESE.Pages;

public class Error404Model : PageModel
{
    public void OnGet()
    {
        Response.StatusCode = 404; // Indica "Not Found" correctamente
    }
}
