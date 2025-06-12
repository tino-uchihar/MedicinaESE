using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicinaESE.Data;
using MedicinaESE.Models;
using System;
using System.Linq;

namespace MedicinaESE.Pages
{
    public class ficha_medicaModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ficha_medicaModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public string Nombre { get; set; } = "";
        [BindProperty]
        public string Apellidos { get; set; } = "";
        [BindProperty]
        public string FechaNacimiento { get; set; } = "";
        [BindProperty]
        public string LugarNacimiento { get; set; } = "";
        [BindProperty]
        public string Genero { get; set; } = "";
        [BindProperty]
        public int Edad { get; set; }
        [BindProperty]
        public float Altura { get; set; }
        [BindProperty]
        public float Peso { get; set; }
        [BindProperty]
        public string GrupoSanguineo { get; set; } = "";
        [BindProperty]
        public string EstadoCivil { get; set; } = "";

        public IActionResult OnGet()
        {
            var documento = HttpContext.Session.GetString("UsuarioDocumento");
            if (string.IsNullOrEmpty(documento))
            {
                return Redirect("/login");
            }

            // Buscar usuario
            var usuario = _db.Usuarios.FirstOrDefault(u => u.DocumentoId == documento);
            if (usuario != null)
            {
                Nombre = usuario.Nombre;
                Apellidos = usuario.Apellido;
            }

            // Buscar paciente
            if (usuario != null)
            {
                var paciente = _db.Pacientes.FirstOrDefault(p => p.IdUsuario == usuario.IdUsuario);
                if (paciente != null)
                {
                    FechaNacimiento = paciente.FechaNacimiento.ToString("yyyy-MM-dd");
                    GrupoSanguineo = paciente.GrupoSanguineo ?? "";
                    EstadoCivil = paciente.EstadoCivil ?? "";
                    // Edad calculada
                    Edad = (int)((DateTime.Now - paciente.FechaNacimiento).TotalDays / 365.25);
                }
            }
            // LugarNacimiento, Genero, Altura, Peso no existen en la BD, se dejan vacíos o con el valor actual
            return Page();
        }

        public void OnPost()
        {
            // Aquí podrías agregar la lógica para almacenar los datos en la base de datos
        }
    }
} 