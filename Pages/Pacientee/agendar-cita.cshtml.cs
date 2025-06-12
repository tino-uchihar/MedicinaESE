// Pages/Pacientee/Agendar-Cita.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedicinaESE.Data;
using MedicinaESE.Models;
using System.ComponentModel.DataAnnotations;


namespace MedicinaESE.Pages.Pacientee   
{
    public partial class AgendarCitaModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public AgendarCitaModel(ApplicationDbContext db) => _db = db;

        // ---------- info del usuario logueado ----------
        public Paciente PacienteActual { get; private set; } = null!;

        // ---------- campos del formulario --------------
        [BindProperty, Required]
        public string AppointmentType { get; set; } = "";

        [BindProperty, Required]
        public int SelectedMedicoId { get; set; }

        [BindProperty, Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [BindProperty, Required]               // formato “HH:mm”
        public string Time { get; set; } = "";

        [BindProperty]
        public string? Notes { get; set; }

        public List<SelectListItem> Profesionales    { get; set; } = new();
        public List<SelectListItem> HorasDisponibles { get; set; } = new();

        public bool   CitaConfirmada { get; set; }
        public string Professional   { get; set; } = "";
        public string Sala           { get; set; } = "";

        // ------------------ GET ------------------------
        public IActionResult OnGet()
        {
            if (!CargarPaciente()) return RedirectToPage("/Cuenta/Login");
            RellenarProfesionales();
            return Page();
        }

        // AJAX: horas disponibles
        public JsonResult OnGetHoras(int medicoId, DateTime fecha)
        {
            var horas = CalcularHuecos(medicoId, fecha)
                        .Select(h => h.ToString(@"hh\:mm"));
            return new JsonResult(horas);
        }

        // ------------------ POST -----------------------
        public IActionResult OnPost()
        {
            if (!CargarPaciente()) return RedirectToPage("/Cuenta/Login");

            if (!ModelState.IsValid)
            {
                RellenarProfesionales();
                return Page();
            }

            var horaSpan = TimeSpan.Parse(Time);
            bool ocupado = _db.Citas.Any(c =>
                c.IdMedico  == SelectedMedicoId &&
                c.FechaCita == Date &&
                c.HoraCita  == horaSpan);

            if (ocupado)
            {
                ModelState.AddModelError(string.Empty, "Ese horario ya fue tomado. Escoge otro.");
                RellenarProfesionales();
                return Page();
            }

            var cita = new Cita
            {
                IdPaciente     = PacienteActual.IdPaciente,
                IdMedico       = SelectedMedicoId,
                FechaCita      = Date,
                HoraCita       = horaSpan,
                EstadoCita     = "Pendiente",
                MotivoConsulta = Notes ?? ""
            };
            _db.Citas.Add(cita);
            _db.SaveChanges();

            var med = _db.Medicos
                         .Include(m => m.Usuario)
                         .First(m => m.IdMedico == SelectedMedicoId);

            Professional   = $"{med.Especialidad} – Dr(a). {med.Usuario.Nombre} {med.Usuario.Apellido}";
            Sala           = med.Consultorio;
            CitaConfirmada = true;
            return Page();
        }

        // ---------------- helpers ----------------------
        private bool CargarPaciente()
        {
            var docId = User.FindFirst("DocumentoId")?.Value;
            PacienteActual = _db.Pacientes
                                .Include(p => p.Usuario)
                                .FirstOrDefault(p => p.Usuario.DocumentoId == docId)!;
            return PacienteActual != null;
        }

        private void RellenarProfesionales()
        {
            var medicos = string.IsNullOrEmpty(AppointmentType)
                ? _db.Medicos.Include(m => m.Usuario).ToList()
                : _db.Medicos.Include(m => m.Usuario)
                             .Where(m => m.Especialidad == AppointmentType).ToList();

            if (!medicos.Any())
            {
                Profesionales = new()
                {
                    new SelectListItem("No hay disponibles", "-1") { Disabled = true }
                };
                return;
            }

            Profesionales = medicos.Select(m => new SelectListItem(
                    $"{m.Especialidad} – Dr(a). {m.Usuario.Nombre} {m.Usuario.Apellido}",
                    m.IdMedico.ToString()))
                                   .ToList();
        }

        private IEnumerable<TimeSpan> CalcularHuecos(int medicoId, DateTime fecha)
        {
            var med = _db.Medicos.First(m => m.IdMedico == medicoId);
            var inicio = med.HorarioInicio;
            var fin    = med.HorarioFin;

            var ocupadas = _db.Citas
                              .Where(c => c.IdMedico == medicoId && c.FechaCita == fecha)
                              .Select(c => c.HoraCita)
                              .ToHashSet();

            for (var t = inicio; t < fin; t += TimeSpan.FromMinutes(30))
                if (!ocupadas.Contains(t))
                    yield return t;
        }
    }
}
