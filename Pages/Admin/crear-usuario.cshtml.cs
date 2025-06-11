using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicinaESE.Data;
using MedicinaESE.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicinaESE.Pages.Admin
{
    public class CrearUsuarioModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CrearUsuarioModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // ----- PROPIEDADES BINDIDAS DEL FORMULARIO -----

        // Datos generales (para todos)
        [BindProperty]
        public string DocumentoId { get; set; } = string.Empty;
        [BindProperty]
        public string Nombre { get; set; } = string.Empty;
        [BindProperty]
        public string Apellido { get; set; } = string.Empty;
        [BindProperty]
        public string? Telefono { get; set; }  // Opcional (null si está en blanco)
        [BindProperty]
        public string Correo { get; set; } = string.Empty;
        [BindProperty]
        public string Contraseña { get; set; } = string.Empty;
        // TipoUsuario: "admin", "medico" o "paciente"
        [BindProperty]
        public string TipoUsuario { get; set; } = string.Empty;

        // Datos para Médico (obligatorios para admin y medico)
        [BindProperty]
        public string Especialidad { get; set; } = string.Empty;
        [BindProperty]
        public string? OtraEspecialidad { get; set; }
        [BindProperty]
        public TimeSpan? HorarioInicio { get; set; }
        [BindProperty]
        public TimeSpan? HorarioFin { get; set; }
        [BindProperty]
        public string Consultorio { get; set; } = string.Empty;

        // Datos para Paciente (obligatorios para todos)
        [BindProperty]
        public DateTime? FechaNacimiento { get; set; }
        [BindProperty]
        public string GrupoSanguineo { get; set; } = string.Empty;
        [BindProperty]
        public string EstadoCivil { get; set; } = string.Empty;
        [BindProperty]
        public string EntidadSalud { get; set; } = string.Empty;
        [BindProperty]
        public string? OtraEntidadSalud { get; set; }
        [BindProperty]
        public string Estado { get; set; } = string.Empty;

        public void OnGet()
        {
            // Carga inicial de la página
        }

        public async Task<IActionResult> OnPostAsync()
        {

            // Validar longitud mínima de contraseña (8)
            if (string.IsNullOrWhiteSpace(Contraseña) || Contraseña.Length < 8)
            {
                ModelState.AddModelError("Contraseña",
                    "La contraseña debe tener al menos 8 caracteres.");
            }
            
            // --- VALIDACIÓN CONDICIONAL SEGÚN TIPO DE USUARIO ---

            // Para "admin" y "medico" se requieren datos de las secciones de Médico y Paciente.
            if (TipoUsuario == "admin" || TipoUsuario == "medico")
            {
                // Validación para la sección de Médico
                if (string.IsNullOrWhiteSpace(Especialidad))
                {
                    ModelState.AddModelError("Especialidad", "La especialidad es obligatoria para admin/medico.");
                }
                if (Especialidad == "Otro" && string.IsNullOrWhiteSpace(OtraEspecialidad))
                {
                    ModelState.AddModelError("OtraEspecialidad", "Debe especificar la especialidad cuando se seleccione 'Otro'.");
                }
                if (!HorarioInicio.HasValue)
                {
                    ModelState.AddModelError("HorarioInicio", "El horario de inicio es obligatorio.");
                }
                if (!HorarioFin.HasValue)
                {
                    ModelState.AddModelError("HorarioFin", "El horario de fin es obligatorio.");
                }
                if (string.IsNullOrWhiteSpace(Consultorio))
                {
                    ModelState.AddModelError("Consultorio", "El consultorio es obligatorio.");
                }

                // Validación para la sección de Paciente
                if (!FechaNacimiento.HasValue)
                {
                    ModelState.AddModelError("FechaNacimiento", "La fecha de nacimiento es obligatoria.");
                }
                if (string.IsNullOrWhiteSpace(GrupoSanguineo))
                {
                    ModelState.AddModelError("GrupoSanguineo", "El grupo sanguíneo es obligatorio.");
                }
                if (string.IsNullOrWhiteSpace(EstadoCivil))
                {
                    ModelState.AddModelError("EstadoCivil", "El estado civil es obligatorio.");
                }
                if (string.IsNullOrWhiteSpace(EntidadSalud))
                {
                    ModelState.AddModelError("EntidadSalud", "La entidad de salud es obligatoria.");
                }
                if (EntidadSalud == "Otro" && string.IsNullOrWhiteSpace(OtraEntidadSalud))
                {
                    ModelState.AddModelError("OtraEntidadSalud", "Debe especificar la entidad de salud cuando se seleccione 'Otro'.");
                }
                if (string.IsNullOrWhiteSpace(Estado))
                {
                    ModelState.AddModelError("Estado", "El estado es obligatorio.");
                }
            }
            // Para "paciente": se exige solo la sección de Paciente.
            else if (TipoUsuario == "paciente")
            {
                if (!FechaNacimiento.HasValue)
                {
                    ModelState.AddModelError("FechaNacimiento", "La fecha de nacimiento es obligatoria.");
                }
                if (string.IsNullOrWhiteSpace(GrupoSanguineo))
                {
                    ModelState.AddModelError("GrupoSanguineo", "El grupo sanguíneo es obligatorio.");
                }
                if (string.IsNullOrWhiteSpace(EstadoCivil))
                {
                    ModelState.AddModelError("EstadoCivil", "El estado civil es obligatorio.");
                }
                if (string.IsNullOrWhiteSpace(EntidadSalud))
                {
                    ModelState.AddModelError("EntidadSalud", "La entidad de salud es obligatoria.");
                }
                if (EntidadSalud == "Otro" && string.IsNullOrWhiteSpace(OtraEntidadSalud))
                {
                    ModelState.AddModelError("OtraEntidadSalud", "Debe especificar la entidad de salud cuando se seleccione 'Otro'.");
                }
                if (string.IsNullOrWhiteSpace(Estado))
                {
                    ModelState.AddModelError("Estado", "El estado es obligatorio.");
                }

                // Remover cualquier error que pudiera haberse agregado para la sección de Médico.
                ModelState.Remove("Especialidad");
                ModelState.Remove("OtraEspecialidad");
                ModelState.Remove("HorarioInicio");
                ModelState.Remove("HorarioFin");
                ModelState.Remove("Consultorio");
            }

            // --- FIN VALIDACIÓN CONDICIONAL ---

            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
                Console.WriteLine("Errores de ModelState: " + errors);
                // Deja el mensaje de error para mostrar en esta misma página (CrearUsuario).
                TempData["Mensaje"] = "Error en el formulario, verifique los datos.";
                return Page();
            }

            // --- PROCESO DE INSERCIÓN ---

            // Verificar si ya existe un usuario con el mismo DocumentoId, Correo o Telefono.
            // Por ejemplo, si deseas evitar duplicados, puedes hacer:
            if (_db.Usuarios.Any(u => u.DocumentoId == DocumentoId))
            {
                ModelState.AddModelError("DocumentoId", "El Documento ya existe.");
                return Page();
            }

            if (_db.Usuarios.Any(u => u.Correo == Correo))
            {
                ModelState.AddModelError("Correo", "El Correo electrónico ya existe.");
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Telefono) && _db.Usuarios.Any(u => u.Telefono == Telefono))
            {
                ModelState.AddModelError("Telefono", "El Teléfono ya existe.");
                return Page();
            }


            // 1) Crear e insertar el usuario en la tabla Usuarios
            var usuario = new Usuario
            {
                DocumentoId = DocumentoId,
                Nombre = Nombre,
                Apellido = Apellido,
                Telefono = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono,
                Correo = Correo,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(Contraseña),
                TipoUsuario = TipoUsuario
            };

            _db.Usuarios.Add(usuario);
            var cambios = await _db.SaveChangesAsync();
            if (cambios == 0)
            {
                TempData["Mensaje"] = "Error al guardar el usuario, intente de nuevo.";
                return Page();
            }

            // 2) Para admin y médico, insertar registro en la tabla Medicos
            if (TipoUsuario == "admin" || TipoUsuario == "medico")
            {
                // Si se selecciona "Otro", se utiliza OtraEspecialidad; se fuerza a que no sea null.
                var especialidadFinal = Especialidad == "Otro" ? (OtraEspecialidad ?? string.Empty) : Especialidad;
                var medico = new Medico
                {
                    IdUsuario = usuario.IdUsuario,
                    Especialidad = especialidadFinal,
                    HorarioInicio = HorarioInicio ?? TimeSpan.Zero,
                    HorarioFin = HorarioFin ?? TimeSpan.Zero,
                    Consultorio = Consultorio
                };
                _db.Medicos.Add(medico);
                await _db.SaveChangesAsync();
            }

            // 3) Insertar registro en la tabla Pacientes (para admin, médico y paciente)
            {
                var entidadSaludFinal = EntidadSalud == "Otro" ? (OtraEntidadSalud ?? string.Empty) : EntidadSalud;
                var paciente = new Paciente
                {
                    IdUsuario = usuario.IdUsuario,
                    FechaNacimiento = FechaNacimiento!.Value,
                    GrupoSanguineo = GrupoSanguineo,
                    EstadoCivil = EstadoCivil,
                    NombreEPS = entidadSaludFinal,
                    Estado = Estado
                };
                _db.Pacientes.Add(paciente);
                await _db.SaveChangesAsync();
            }

            TempData["Mensaje"] = $"Se ha creado el usuario: {DocumentoId}";
            return RedirectToPage("/admin-main");
        }
    }
}
