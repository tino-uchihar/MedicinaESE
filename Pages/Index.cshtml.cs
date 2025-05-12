using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MedicinaESE.Services;
using MedicinaESE.Models;
using System.Collections.Generic;

namespace MedicinaESE.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly NoticiasService _noticiasService; // 

    public List<Noticia> Noticias { get; set; } = new List<Noticia>();

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        _noticiasService = new NoticiasService(); // 
    }

    public void OnGet()
    {
        Noticias = _noticiasService.ObtenerNoticias();

        if (Noticias.Count == 0)
        {
            _logger.LogWarning("⚠️ No se encontraron noticias."); // 
        }
    }
}
