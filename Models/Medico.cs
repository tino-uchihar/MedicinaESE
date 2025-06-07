namespace MedicinaESE.Models
{
    public class Medico
    {
        // Esto es lo que EF Core buscará por convención
        public int      IdMedico     { get; set; }
        public int      IdUsuario    { get; set; }
        public string   Especialidad { get; set; } = null!;
        public TimeSpan HorarioInicio{ get; set; }
        public TimeSpan HorarioFin   { get; set; }
        public string   Consultorio  { get; set; } = null!;
    }
}
