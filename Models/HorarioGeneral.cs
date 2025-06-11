// Models/HorarioGeneral.cs
namespace MedicinaESE.Models
{
    public class HorarioGeneral
    {
        public int      IdHorarioGeneral { get; set; }
        public string   Descripcion      { get; set; } = string.Empty;
        public TimeSpan HoraInicio       { get; set; }
        public TimeSpan HoraFin          { get; set; }

        public ICollection<AsignacionHorario> Asignaciones { get; set; } = new List<AsignacionHorario>();
    }
}
