using Microsoft.AspNetCore.Mvc; // Agregado para usar IActionResult
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MedicinaESE.Services;
using MedicinaESE.Models;
using System.Collections.Generic;

namespace MedicinaESE.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly NoticiaService _noticiasService; // Servicio de noticias

    public List<Noticia> Noticias { get; set; } = new List<Noticia>();

    // Inyección del servicio en el constructor
    public IndexModel(ILogger<IndexModel> logger, NoticiaService noticiasService)
    {
        _logger = logger;
        _noticiasService = noticiasService;
    }

    // Manejo de conexión fallida con la base de datos
    public IActionResult OnGet()
    {
        try
        {
            Noticias = _noticiasService.ObtenerNoticias();

            if (Noticias.Count == 0)
            {
                _logger.LogWarning("No se encontraron noticias.");
            }

            return Page(); // Devuelve la vista normal si todo está bien
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al obtener noticias: {ex.Message}");
            return RedirectToPage("/ErrorBD"); // Redirige a la página de error
        }
    }
}
