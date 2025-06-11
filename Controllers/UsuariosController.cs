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

        [HttpDelete("{documentoId}")]
        public IActionResult Delete(string documentoId)
        {
            var u = _db.Usuarios.FirstOrDefault(x => x.DocumentoId == documentoId);
            if (u == null) return NotFound();

            var med = _db.Medicos.FirstOrDefault(m => m.IdUsuario == u.IdUsuario);
            var pac = _db.Pacientes.FirstOrDefault(p => p.IdUsuario == u.IdUsuario);
            if (med != null) _db.Medicos.Remove(med);
            if (pac != null) _db.Pacientes.Remove(pac);

            _db.Usuarios.Remove(u);
            _db.SaveChanges();
            return NoContent();      // 204
        }
        
                // DTO mínimo para la edición (solo los campos que realmente cambias desde la vista)
        public class UsuarioUpdateDto
        {
            public string Nombre       { get; set; } = "";
            public string Apellido     { get; set; } = "";
            public string Correo       { get; set; } = "";
            public string? Telefono    { get; set; }
            public string? Contraseña  { get; set; }   // ← puede venir null o vacía
        }

        [HttpPut("{documentoId}")]
        public IActionResult Put(string documentoId, [FromBody] UsuarioUpdateDto dto)
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.DocumentoId == documentoId);
            if (usuario == null) return NotFound();

            // 1) Actualizar datos básicos
            usuario.Nombre   = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Correo   = dto.Correo;
            usuario.Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono;

            // 2) Validar y aplicar nueva contraseña (si la enviaron)
            if (!string.IsNullOrWhiteSpace(dto.Contraseña))
            {
                if (dto.Contraseña.Length < 8)
                    return BadRequest("La contraseña debe tener al menos 8 caracteres.");   // ← aquí va el BadRequest

                usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(dto.Contraseña);
            }

            _db.SaveChanges();
            return NoContent();   // 204 éxito sin cuerpo
        }


    }
}
