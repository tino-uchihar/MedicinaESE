using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MedicinaESE.Services;
using MedicinaESE.Models;
using System.Collections.Generic;

namespace MedicinaESE.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly NoticiaService _noticiasService; // Inyectamos el servicio correctamente

    public List<Noticia> Noticias { get; set; } = new List<Noticia>();

    // Inyección del servicio en el constructor
    public IndexModel(ILogger<IndexModel> logger, NoticiaService noticiasService)
    {
        _logger = logger;
        _noticiasService = noticiasService; // Asignamos la instancia inyectada
    }

    public void OnGet()
    {
        Noticias = _noticiasService.ObtenerNoticias();

        if (Noticias.Count == 0)
        {
            _logger.LogWarning("No se encontraron noticias.");
        }
    }
}
