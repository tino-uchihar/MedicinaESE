using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicinaESE.Pages
{
    [Authorize(Policy = "RequireMedico")]
    public class medico_mainModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
} 