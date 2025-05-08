using System.Collections.Generic;
using System.Linq;
using MedicinaESE.Models;

namespace MedicinaESE.Services;

public class NoticiaService
{
    private static List<Noticia> noticias = new List<Noticia>
    {
        new Noticia { Id = 1, Titulo = "Título 1", Descripcion = "Descripción corta.", ImagenUrl = "/img/noticias/noticia1.jpg", Enlace = "#" },
        new Noticia { Id = 2, Titulo = "Título 2", Descripcion = "Otra noticia interesante.", ImagenUrl = "/img/noticias/noticia2.jpg", Enlace = "#" },
        new Noticia { Id = 3, Titulo = "Título 3", Descripcion = "Más información relevante.", ImagenUrl = "/img/noticias/noticia3.jpg", Enlace = "#" },
        new Noticia { Id = 4, Titulo = "Título 4", Descripcion = "Nueva noticia agregada.", ImagenUrl = "/img/noticias/noticia4.jpg", Enlace = "#" }
    };

    public List<Noticia> ObtenerNoticias()
    {
        return noticias;
    }

    public void AgregarNoticia(Noticia nuevaNoticia)
    {
        nuevaNoticia.Id = noticias.Count + 1; // Generar ID automático
        noticias.Add(nuevaNoticia);
    }

    public void EliminarNoticia(int id)
    {
        var noticiaAEliminar = noticias.FirstOrDefault(n => n.Id == id);
        if (noticiaAEliminar != null)
        {
            noticias.Remove(noticiaAEliminar);
        }
    }
}
