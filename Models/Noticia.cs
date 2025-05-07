namespace MedicinaESE.Models;

public class Noticia
{
    public int Id { get; set; } // Identificador único
    public string Titulo { get; set; } = ""; // Título de la noticia 
    public string Descripcion { get; set; } = ""; // Descripción corta
    public string ImagenUrl { get; set; } = ""; // URL de la imagen
    public string Enlace { get; set; } = ""; // Link a la noticia completa
}
