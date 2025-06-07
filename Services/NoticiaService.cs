using System.Collections.Generic;
using System.Linq;
using MedicinaESE.Data;
using MedicinaESE.Models;

namespace MedicinaESE.Services
{
    // Servicio para gestionar noticias obtenidas desde la BBDD con EF Core
    public class NoticiaService
    {
        private readonly ApplicationDbContext _db;

        // Inyectamos el DbContext para acceder a las tablas
        public NoticiaService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Obtiene todas las noticias, ordenadas por Fecha de publicación descendente
        public List<Noticia> ObtenerNoticias()
        {
            // 1) _db.Noticias accede a la tabla Noticias
            // 2) OrderByDescending para mostrar primero las más recientes
            // 3) ToList ejecuta la consulta y materializa la lista
            return _db.Noticias
                      .OrderByDescending(n => n.FechaPublicacion)
                      .ToList();
        }
    }
}
