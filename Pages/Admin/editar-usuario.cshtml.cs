using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MedicinaESE.Data;      // Ajusta el namespace de tu ApplicationDbContext
using MedicinaESE.Models;    // Ajusta el namespace de tus modelos

namespace MedicinaESE.Pages.Admin
{
    public class EditarUsuarioModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EditarUsuarioModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public EditarUsuarioViewModel EditarUsuario { get; set; } = new();

        public IActionResult OnGet(string documentoId)
        {
            if (string.IsNullOrEmpty(documentoId))
            {
                return NotFound();
            }

            // Buscamos el usuario en la tabla Usuarios.
            var usuario = _db.Usuarios.FirstOrDefault(u => u.DocumentoId == documentoId);
            if (usuario == null)
            {
                return NotFound();
            }

            // Asigna los datos generales, usando el operador de coalescencia para evitar null.
            EditarUsuario.DocumentoId = usuario.DocumentoId;
            EditarUsuario.Nombre = usuario.Nombre;
            EditarUsuario.Apellido = usuario.Apellido;
            EditarUsuario.Correo = usuario.Correo;
            EditarUsuario.Telefono = usuario.Telefono ?? "";
            EditarUsuario.Contraseña = usuario.Contraseña;
            EditarUsuario.TipoUsuario = usuario.TipoUsuario;

            // Si es "medico" o "admin" se consultan los datos de la tabla Medicos
            if (usuario.TipoUsuario.Equals("medico", System.StringComparison.OrdinalIgnoreCase) ||
                usuario.TipoUsuario.Equals("admin", System.StringComparison.OrdinalIgnoreCase))
            {
                var medico = _db.Medicos.FirstOrDefault(m => m.IdUsuario == usuario.IdUsuario);
                if (medico != null)
                {
                    EditarUsuario.Especialidad = medico.Especialidad ?? "";
                    // Se asume que si el valor real no está en la lista predeterminada, la vista mostrará "Otro" de forma automática.
                    EditarUsuario.HorarioInicio = medico.HorarioInicio;
                    EditarUsuario.HorarioFin = medico.HorarioFin;
                    EditarUsuario.Consultorio = medico.Consultorio ?? "";
                }
            }

            // Si es "paciente" o "admin" se consultan los datos de la tabla Pacientes
            if (usuario.TipoUsuario.Equals("paciente", System.StringComparison.OrdinalIgnoreCase) ||
                usuario.TipoUsuario.Equals("admin", System.StringComparison.OrdinalIgnoreCase))
            {
                var paciente = _db.Pacientes.FirstOrDefault(p => p.IdUsuario == usuario.IdUsuario);
                if (paciente != null)
                {
                    EditarUsuario.FechaNacimiento = paciente.FechaNacimiento;
                    EditarUsuario.GrupoSanguineo = paciente.GrupoSanguineo ?? "";
                    EditarUsuario.EstadoCivil = paciente.EstadoCivil ?? "";
                    // En la tabla Pacientes, el campo para la EPS es "NombreEPS"
                    EditarUsuario.EntidadSalud = paciente.NombreEPS ?? "";
                    EditarUsuario.Estado = paciente.Estado;
                }
            }
            return Page();
        }
    }

    public class EditarUsuarioViewModel
    {
        // Datos generales
        public string DocumentoId { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;

        // Datos para Médico
        public string Especialidad { get; set; } = string.Empty;
        public string OtraEspecialidad { get; set; } = string.Empty;
        public TimeSpan? HorarioInicio { get; set; }
        public TimeSpan? HorarioFin { get; set; }
        public string Consultorio { get; set; } = string.Empty;

        // Datos para Paciente
        public DateTime? FechaNacimiento { get; set; }
        public string GrupoSanguineo { get; set; } = string.Empty;
        public string EstadoCivil { get; set; } = string.Empty;
        public string EntidadSalud { get; set; } = string.Empty; // Llenar con NombreEPS
        public string OtraEntidadSalud { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
