using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Medicinaese.Pages
{
    public class ficha_medicaModel : PageModel
    {
        [BindProperty]
        public string Nombre { get; set; } = "";
        [BindProperty]
        public string Apellidos { get; set; } = "";
        [BindProperty]
        public string FechaNacimiento { get; set; } = "";
        [BindProperty]
        public string LugarNacimiento { get; set; } = "";
        [BindProperty]
        public string Genero { get; set; } = "";
        [BindProperty]
        public int Edad { get; set; }
        [BindProperty]
        public float Altura { get; set; }
        [BindProperty]
        public float Peso { get; set; }
        [BindProperty]
        public string GrupoSanguineo { get; set; } = "";
        [BindProperty]
        public string EstadoCivil { get; set; } = "";

        public void OnPost()
        {
            // Aquí podrías agregar la lógica para almacenar los datos en la base de datos
        }
    }
}
