namespace MedicinaESE.Models
{
    // Representa un registro de la tabla Noticias
    public class Noticia
    {
        public int      Id               { get; set; }   // PK Identity
        public string   Titulo           { get; set; } = null!;
        public string   Descripcion      { get; set; } = null!;
        public string   ImagenUrl        { get; set; } = null!;
        public DateTime FechaPublicacion { get; set; }    // Fecha y hora
    }
}
