using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicinaESE.Data;
using MedicinaESE.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicinaESE.Pages.Admin
{
    public class ListaUsuariosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<UsuarioModel> Usuarios { get; set; } = new();

        public ListaUsuariosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Usando EF Core, se obtiene la lista de usuarios actualizada
            Usuarios = _context.Usuarios
                .Select(u => new UsuarioModel
                {
                    DocumentoId = u.DocumentoId,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Telefono = u.Telefono ?? "",
                    TipoUsuario = u.TipoUsuario
                })
                .ToList();
        }
    }

    public class UsuarioModel
    {
        public string DocumentoId { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string TipoUsuario { get; set; } = "";
    }
}
