namespace MedicinaESE.Models
{
  public class Paciente
  {
      public int  IdPaciente  { get; set; }

      public int  IdUsuario   { get; set; }          // FK
      public Usuario Usuario  { get; set; } = null!; // navegación

      public DateTime FechaNacimiento { get; set; }
      public string  GrupoSanguineo   { get; set; } = "";
      public string  EstadoCivil      { get; set; } = "";
      public string  NombreEPS        { get; set; } = "";
      public string  Estado           { get; set; } = "Activo";
  }

}
