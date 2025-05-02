using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Medicinaese.Pages
{
    public class agendar_citaModel : PageModel
    {
        [BindProperty]
        public string AppointmentType { get; set; } = "";
        [BindProperty]
        public string PatientName { get; set; } = "";
        [BindProperty]
        public string IdType { get; set; } = "";
        [BindProperty]
        public string IdNumber { get; set; } = "";
        [BindProperty]
        public string Professional { get; set; } = "";
        [BindProperty]
        public string Room { get; set; } = "";
        [BindProperty]
        public string Date { get; set; } = "";
        [BindProperty]
        public string Time { get; set; } = "";
        [BindProperty]
        public string Notes { get; set; } = "";

        public void OnPost()
        {
            // Aquí podrías agregar la lógica para guardar la cita en una base de datos.
        }
    }
}
