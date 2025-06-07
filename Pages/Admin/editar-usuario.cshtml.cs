using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace MedicinaESE.Pages.Admin
{
    public class EditarUsuarioModel : PageModel
    {
        [BindProperty]
        public EditarUsuarioViewModel EditarUsuario { get; set; } = new();

        public void OnGet(string documentoId)
        {
            // Si se navega tradicionalmente a esta página, aquí se podría cargar la información 
            // correspondiente al 'documentoId' y asignarla a 'EditarUsuario'. Ejemplo:
            //
            // EditarUsuario = _db.Usuarios.Where(u => u.DocumentoId == documentoId)
            //     .Select(u => new EditarUsuarioViewModel
            //     {
            //         DocumentoId = u.DocumentoId,
            //         Nombre = u.Nombre,
            //         Apellido = u.Apellido,
            //         Correo = u.Correo,
            //         Telefono = u.Telefono ?? "",
            //         Contraseña = u.Contraseña,
            //         TipoUsuario = u.TipoUsuario,
            //         Especialidad = u.Medico.Especialidad,
            //         OtraEspecialidad = u.Medico.OtraEspecialidad,
            //         HorarioInicio = u.Medico.HorarioInicio,
            //         HorarioFin = u.Medico.HorarioFin,
            //         Consultorio = u.Medico.Consultorio,
            //         FechaNacimiento = u.Paciente.FechaNacimiento,
            //         GrupoSanguineo = u.Paciente.GrupoSanguineo,
            //         EstadoCivil = u.Paciente.EstadoCivil,
            //         EntidadSalud = u.Paciente.EntidadSalud,
            //         OtraEntidadSalud = u.Paciente.OtraEntidadSalud,
            //         Estado = u.Paciente.Estado
            //     }).FirstOrDefault();
            //
            // Si la carga se realiza vía Fetch, este método se puede dejar vacío.
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

        // Datos para Médico (aplica si el TipoUsuario es "admin" o "medico")
        public string Especialidad { get; set; } = string.Empty;
        public string OtraEspecialidad { get; set; } = string.Empty;
        public TimeSpan? HorarioInicio { get; set; }
        public TimeSpan? HorarioFin { get; set; }
        public string Consultorio { get; set; } = string.Empty;

        // Datos para Paciente
        public DateTime? FechaNacimiento { get; set; }
        public string GrupoSanguineo { get; set; } = string.Empty;
        public string EstadoCivil { get; set; } = string.Empty;
        public string EntidadSalud { get; set; } = string.Empty;
        public string OtraEntidadSalud { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
