using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicinaESE.Pages
{
    [Authorize(Policy = "RequirePaciente")]
    public class paciente_mainModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
} 