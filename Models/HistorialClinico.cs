// Models/HistorialClinico.cs
namespace MedicinaESE.Models
{
    public class HistorialClinico
    {
        public int     IdHistorial      { get; set; }

        public int     IdCita           { get; set; }
        public Cita?   Cita             { get; set; }

        public string  Diagnostico      { get; set; } = string.Empty;
        public string? RecetaMedica     { get; set; }
        public string? NotasAdicionales { get; set; }
    }
}
