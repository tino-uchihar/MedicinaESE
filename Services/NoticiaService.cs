using System;
using System.Collections.Generic;
using MedicinaESE.Models;


namespace MedicinaESE.Services;

public class NoticiaService
{
    public List<Noticia> ObtenerNoticias()
    {
        return new List<Noticia>
        {
            new Noticia { Id = 1, Titulo = "Avances Médicos", Descripcion = "Descubren nueva terapia genética.", ImagenUrl = "/img/noticias/noticia1.jpg", FechaPublicacion = DateTime.Now.AddDays(-2) },
            new Noticia { Id = 2, Titulo = "Salud y Bienestar", Descripcion = "Consejos para mejorar la calidad de vida.", ImagenUrl = "/img/noticias/noticia2.jpg", FechaPublicacion = DateTime.Now.AddDays(-5) },
            new Noticia { Id = 3, Titulo = "Nutrición Inteligente", Descripcion = "Alimentación saludable según expertos.", ImagenUrl = "/img/noticias/noticia3.jpg", FechaPublicacion = DateTime.Now.AddDays(-7) },
            new Noticia { Id = 4, Titulo = "Ejercicio y Salud", Descripcion = "Impacto positivo del deporte en la salud.", ImagenUrl = "/img/noticias/noticia4.jpg", FechaPublicacion = DateTime.Now.AddDays(-10) }
        };
    }
}
