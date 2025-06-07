using System.Linq;
using MedicinaESE.Data;
using BCrypt.Net;

namespace MedicinaESE.Services
{
    // Servicio de autenticación que valida credenciales contra la BBDD con EF Core
    public class AuthService
    {
        private readonly ApplicationDbContext _db;

        // Inyectamos el DbContext en lugar de SqlConnection
        public AuthService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Valida el documento + contraseña
        public (bool Exitoso, string Mensaje, string Nombre, string Correo, string TipoUsuario)
            ValidarCredenciales(string documento, string contraseña)
        {
            // 1) Buscamos el usuario por DocumentoId
            var usuario = _db.Usuarios
                             .Where(u => u.DocumentoId == documento)
                             .Select(u => new {
                                 u.Nombre,
                                 u.Correo,
                                 u.Contraseña,   // hash almacenado
                                 u.TipoUsuario
                             })
                             .FirstOrDefault();

            // 2) Si no existe, devolvemos error
            if (usuario == null)
                return (false, "usuarioNoEncontrado", "", "", "");

            // 3) Verificamos la contraseña con BCrypt
            bool esValido = BCrypt.Net.BCrypt.Verify(contraseña, usuario.Contraseña);
            if (!esValido)
                return (false, "contraIncorrecta", "", "", "");

            // 4) Credenciales válidas
            return (true, "success",
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.TipoUsuario);
        }
    }
}
