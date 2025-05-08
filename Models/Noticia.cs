namespace MedicinaESE.Models;

public class Noticia
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public string ImagenUrl { get; set; }
    public DateTime FechaPublicacion { get; set; }

    // Constructor opcional
    public Noticia(int id, string titulo, string descripcion, string imagenUrl, DateTime fechaPublicacion)
    {
        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
        ImagenUrl = imagenUrl;
        FechaPublicacion = fechaPublicacion;
    }
}
