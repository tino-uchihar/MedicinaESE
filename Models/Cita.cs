// Models/Cita.cs
namespace MedicinaESE.Models
{
    public class Cita
    {
        public int      IdCita      { get; set; }

        public int      IdPaciente  { get; set; }
        public Paciente? Paciente   { get; set; }

        public int      IdMedico    { get; set; }
        public Medico?  Medico      { get; set; }

        public DateTime FechaCita   { get; set; }
        public TimeSpan HoraCita    { get; set; }

        public string   EstadoCita  { get; set; } = "Pendiente";   // Pendiente|Confirmada|Cancelada
        public string   MotivoConsulta { get; set; } = string.Empty;

        public ICollection<HistorialClinico> Historiales { get; set; } = new List<HistorialClinico>();
    }
}
