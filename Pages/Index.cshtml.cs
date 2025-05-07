using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MedicinaESE.Services;
using MedicinaESE.Models;
using System.Collections.Generic;

namespace MedicinaESE.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly NoticiaService _noticiaService;

    public List<Noticia> Noticias { get; set; } = new List<Noticia>();

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        _noticiaService = new NoticiaService();
    }

    public void OnGet()
    {
        Noticias = _noticiaService.ObtenerNoticias();

        if (Noticias.Count == 0)
        {
            Console.WriteLine("⚠️ No se encontraron noticias.");
        }
    }

}
