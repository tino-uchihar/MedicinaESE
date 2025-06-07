using Microsoft.AspNetCore.Mvc;
using MedicinaESE.Data;
using MedicinaESE.Models;
using System.Linq;

namespace MedicinaESE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UsuariosController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetUserInfo")]
        public IActionResult GetUserInfo(string documentoId)
        {
            // Obtener el usuario
            var usuario = _db.Usuarios.FirstOrDefault(u => u.DocumentoId == documentoId);
            if (usuario == null)
            {
                return NotFound();
            }

            // Obtener la información específica: si el usuario es admin o médico, traer su información médica.
            Medico? medico = null;  // Nota el '?' para marcarlo como nullable
            if (usuario.TipoUsuario == "admin" || usuario.TipoUsuario == "medico")
            {
                medico = _db.Medicos.FirstOrDefault(m => m.IdUsuario == usuario.IdUsuario);
            }

            // Siempre obtener la información del paciente.
            var paciente = _db.Pacientes.FirstOrDefault(p => p.IdUsuario == usuario.IdUsuario);

            return Ok(new 
            { 
                usuario,
                medico,
                paciente 
            });
        }


        // Aquí podrás agregar otros endpoints para actualizar, guardar cambios, etc.
    }
}
