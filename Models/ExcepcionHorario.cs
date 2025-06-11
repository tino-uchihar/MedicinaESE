// Models/ExcepcionHorario.cs
namespace MedicinaESE.Models
{
    public class ExcepcionHorario
    {
        public int      IdExcepcion        { get; set; }

        public int      IdMedico           { get; set; }
        public Medico?  Medico             { get; set; }

        public DateTime Fecha              { get; set; }
        public TimeSpan? HoraInicioEspecial{ get; set; }
        public TimeSpan? HoraFinEspecial   { get; set; }
        public bool     Cancelado          { get; set; } = false;
    }
}
