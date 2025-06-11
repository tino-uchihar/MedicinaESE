// Models/AsignacionHorario.cs
using System.ComponentModel.DataAnnotations;

namespace MedicinaESE.Models
{
    public class AsignacionHorario
    {
        public int IdAsignacion { get; set; }

        public int IdMedico { get; set; }
        public Medico? Medico { get; set; }

        public int IdHorarioGeneral { get; set; }
        public HorarioGeneral? HorarioGeneral { get; set; }

        public DateTime FechaInicioSemana { get; set; }
        public DateTime FechaFinSemana { get; set; }
    }
}
