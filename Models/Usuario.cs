namespace MedicinaESE.Models
{
  public class Usuario
  {
    public int    IdUsuario   { get; set; }
    public string DocumentoId { get; set; } = null!;
    public string Nombre      { get; set; } = null!;
    public string Apellido    { get; set; } = null!;
    public string? Telefono   { get; set; }
    public string Correo      { get; set; } = null!;
    public string Contraseña  { get; set; } = null!;
    public string TipoUsuario { get; set; } = null!;
  }
}
