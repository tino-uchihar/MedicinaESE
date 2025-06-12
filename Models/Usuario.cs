namespace MedicinaESE.Models
{
  public class Usuario
  {
    public int IdUsuario { get; set; }
    public string DocumentoId { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string? Telefono { get; set; }
    public string Correo { get; set; } = null!;
    public string Contraseña { get; set; } = null!;
    public string TipoUsuario { get; set; } = null!;
      
    public ICollection<Medico>   Medicos   { get; set; } = new List<Medico>();
    public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();


  }
}
