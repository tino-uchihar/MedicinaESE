using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace MedicinaESE.Pages.Admin
{
    public class CrearUsuarioModel : PageModel
    {
        // Datos generales del usuario
        [BindProperty]
        public string DocumentoId { get; set; } = string.Empty;
        [BindProperty]
        public string Nombre { get; set; } = string.Empty;
        [BindProperty]
        public string Apellido { get; set; } = string.Empty;
        [BindProperty]
        public string Telefono { get; set; } = string.Empty;
        [BindProperty]
        public string Correo { get; set; } = string.Empty;
        [BindProperty]
        public string Contraseña { get; set; } = string.Empty;
        [BindProperty]
        public string TipoUsuario { get; set; } = string.Empty;

        // Datos para Médico
        [BindProperty]
        public string Especialidad { get; set; } = string.Empty;
        [BindProperty]
        public string OtraEspecialidad { get; set; } = string.Empty;
        [BindProperty]
        public TimeSpan? HorarioInicio { get; set; }
        [BindProperty]
        public TimeSpan? HorarioFin { get; set; }
        [BindProperty]
        public string Consultorio { get; set; } = string.Empty;

        // Datos para Paciente
        [BindProperty]
        public DateTime? FechaNacimiento { get; set; }
        [BindProperty]
        public string GrupoSanguineo { get; set; } = string.Empty;
        [BindProperty]
        public string EstadoCivil { get; set; } = string.Empty;
        [BindProperty]
        public string EntidadSalud { get; set; } = string.Empty;
        [BindProperty]
        public string OtraEntidadSalud { get; set; } = string.Empty;
        [BindProperty]
        public string Estado { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Aquí deberás agregar la lógica para insertar en la base de datos.
            // Valida y, en función de TipoUsuario, inserta en Usuarios y luego, si corresponde, en Médicos y/o Pacientes.
            // Para los campos "Otro" en Especialidad y Entidad de Salud, se debe usar el valor ingresado si se seleccionó "Otro".
            return RedirectToPage("admin-main");
        }
    }
}
