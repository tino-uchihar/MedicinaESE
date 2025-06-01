using Microsoft.Data.SqlClient;
using BCrypt.Net;

namespace MedicinaESE.Services;

public class AuthService
{
    private readonly SqlConnection _connection;

    public AuthService(SqlConnection connection)
    {
        _connection = connection;
    }

    public (bool Exitoso, string Mensaje, string Nombre, string Correo, string TipoUsuario) ValidarCredenciales(string documento, string contraseña)
    {
        _connection.Open();
        Console.WriteLine($"Buscando usuario con DocumentoId: {documento}");

        using SqlCommand command = new SqlCommand("SELECT Nombre, Correo, Contraseña, TipoUsuario FROM Usuarios WHERE DocumentoId = @Documento", _connection);
        command.Parameters.AddWithValue("@Documento", documento);

        using SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            string nombre = reader.GetString(0);
            string correo = reader.GetString(1);
            string hashGuardado = reader.GetString(2);
            string tipoUsuario = reader.GetString(3);

            Console.WriteLine($"Usuario encontrado: {nombre}, Tipo: {tipoUsuario}");

            if (BCrypt.Net.BCrypt.Verify(contraseña, hashGuardado))
            {
                Console.WriteLine("Contraseña correcta");
                _connection.Close();
                return (true, "success", nombre, correo, tipoUsuario);
            }
            else
            {
                Console.WriteLine("Contraseña incorrecta");
                _connection.Close();
                return (false, "contraIncorrecta", "", "", "");
            }
        }

        Console.WriteLine("Usuario no encontrado");
        _connection.Close();
        return (false, "usuarioNoEncontrado", "", "", "");
    }
}
