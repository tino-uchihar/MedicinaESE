using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace MedicinaESE.Pages.Admin;

public class ListaUsuariosModel : PageModel
{
    public List<UsuarioModel> Usuarios { get; set; } = new();

    private readonly SqlConnection _connection;

    public ListaUsuariosModel(SqlConnection connection)
    {
        _connection = connection;
    }

    public void OnGet()
    {
        _connection.Open();

        using SqlCommand command = new SqlCommand("SELECT DocumentoId, Nombre, Correo, TipoUsuario FROM Usuarios", _connection);
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Usuarios.Add(new UsuarioModel
            {
                DocumentoId = reader.GetString(0),
                Nombre = reader.GetString(1),
                Correo = reader.GetString(2),
                TipoUsuario = reader.GetString(3)
            });
        }

        _connection.Close();
    }
}

public class UsuarioModel
{
    public string DocumentoId { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Correo { get; set; } = "";
    public string TipoUsuario { get; set; } = "";
}
