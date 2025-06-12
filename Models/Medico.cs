namespace MedicinaESE.Models
{
    public class Medico
    {
        public int  IdMedico    { get; set; }

        public int  IdUsuario   { get; set; }          // FK
        public Usuario Usuario  { get; set; } = null!; // navegación

        public string Especialidad   { get; set; } = "";
        public TimeSpan HorarioInicio{ get; set; }
        public TimeSpan HorarioFin   { get; set; }
        public string   Consultorio  { get; set; } = "";

        // ——— getter calculado ———
        public string NombreCompleto => $"{Usuario.Nombre} {Usuario.Apellido}";
    }

}
