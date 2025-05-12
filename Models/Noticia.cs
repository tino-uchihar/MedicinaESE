namespace MedicinaESE.Models;

public class Noticia
{
    public int Id { get; set; }
    public required string Titulo { get; set; }
    public required string Descripcion { get; set; }
    public required string ImagenUrl { get; set; }
    public required DateTime FechaPublicacion { get; set; }
}
