using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using BCrypt.Net;

namespace MedicinaESE.Pages;

public class RegistroUsuarioModel : PageModel
{
    [BindProperty] public string DocumentoId { get; set; } = string.Empty;
    [BindProperty] public string Nombre { get; set; } = string.Empty;
    [BindProperty] public string Correo { get; set; } = string.Empty;
    [BindProperty] public string Contraseña { get; set; } = string.Empty;
    [BindProperty] public string TipoUsuario { get; set; } = "paciente";

    private readonly SqlConnection _connection;

    public RegistroUsuarioModel(SqlConnection connection)
    {
        _connection = connection;
    }

    public IActionResult OnPost()
    {
        // ✅ Validar si el usuario actual es administrador
        if (HttpContext.Session.GetString("UsuarioTipo") != "admin")
        {
            return RedirectToPage("/ErrorBD"); // Redirigir si no es admin
        }

        try
        {
            _connection.Open();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Contraseña); // Encriptar contraseña

            using SqlCommand command = new SqlCommand(
                "INSERT INTO Usuarios (DocumentoId, Nombre, Correo, Contraseña, TipoUsuario) VALUES (@DocumentoId, @Nombre, @Correo, @Contraseña, @TipoUsuario)",
                _connection);

            command.Parameters.AddWithValue("@DocumentoId", DocumentoId);
            command.Parameters.AddWithValue("@Nombre", Nombre);
            command.Parameters.AddWithValue("@Correo", Correo);
            command.Parameters.AddWithValue("@Contraseña", hashedPassword);
            command.Parameters.AddWithValue("@TipoUsuario", TipoUsuario);

            command.ExecuteNonQuery();
            _connection.Close();

            return RedirectToPage("/perfil-usuario"); // ✅ Redirigir tras éxito
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al registrar usuario: {ex.Message}");
            return RedirectToPage("/ErrorBD"); // 🚨 Redirigir en caso de error
        }
    }
}
