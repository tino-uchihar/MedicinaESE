using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicinaESE.Data;
using System;
using System.Linq;

namespace MedicinaESE.Pages.Medicoo
{
    public class medico_mainModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public medico_mainModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public string Especialidad { get; set; } = "";
        public string HorarioInicio { get; set; } = "";
        public string HorarioFin { get; set; } = "";
        public string Consultorio { get; set; } = "";

        public IActionResult OnGet()
        {
            var documento = HttpContext.Session.GetString("UsuarioDocumento");
            if (string.IsNullOrEmpty(documento))
            {
                return Redirect("/login");
            }

            var usuario = _db.Usuarios.FirstOrDefault(u => u.DocumentoId == documento);
            if (usuario == null)
            {
                return Redirect("/login");
            }

            var medico = _db.Medicos.FirstOrDefault(m => m.IdUsuario == usuario.IdUsuario);
            if (medico != null)
            {
                Especialidad = medico.Especialidad;
                HorarioInicio = medico.HorarioInicio.ToString(@"hh\:mm");
                HorarioFin = medico.HorarioFin.ToString(@"hh\:mm");
                Consultorio = medico.Consultorio;
            }

            return Page();
        }
    }
} 