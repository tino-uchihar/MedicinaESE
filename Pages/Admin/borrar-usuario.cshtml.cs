using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicinaESE.Data;
using MedicinaESE.Models;

namespace MedicinaESE.Pages.Admin
{
    public class BorrarUsuarioModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public BorrarUsuarioModel(ApplicationDbContext db) => _db = db;

        public string DocumentoId    { get; private set; } = "";
        public string NombreCompleto { get; private set; } = "";

        public IActionResult OnGet(string documentoId)
        {
            var u = _db.Usuarios.FirstOrDefault(x => x.DocumentoId == documentoId);
            if (u == null) return NotFound();
            DocumentoId    = u.DocumentoId;
            NombreCompleto = $"{u.Nombre} {u.Apellido}";
            return Page();                       // devuelve solo el fragmento HTML
        }
    }

}
