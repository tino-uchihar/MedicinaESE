using System.Collections.Generic;
using System.Linq;
using MedicinaESE.Models;

namespace MedicinaESE.Services
{
    public class NoticiasService
    {
        private List<Noticia> noticias = new List<Noticia>
        {
            new Noticia(1, "Avance Médico", "Nueva terapia revolucionaria para X condición...", "/img/noticias/noticia1.jpg", DateTime.Now),
            new Noticia(2, "Investigación Científica", "Descubrimiento innovador que cambiará la medicina...", "/img/noticias/noticia2.jpg", DateTime.Now.AddDays(-1)),
            new Noticia(3, "Conferencia Internacional", "Expertos discuten los avances en tratamientos...", "/img/noticias/noticia3.jpg", DateTime.Now.AddDays(-2)),
            new Noticia(4, "Salud Pública", "Nueva normativa sobre el uso de fármacos...", "/img/noticias/noticia4.jpg", DateTime.Now.AddDays(-3))
        };

        public List<Noticia> ObtenerNoticias()
        {
            return noticias;
        }

        public Noticia ObtenerNoticiaPorId(int id)
        {
            return noticias.FirstOrDefault(n => n.Id == id);
        }
    }
}
