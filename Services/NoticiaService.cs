using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MedicinaESE.Models;

namespace MedicinaESE.Services;

public class NoticiaService
{
    private readonly string _connectionString;

    public NoticiaService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Noticia> ObtenerNoticias()
    {
        List<Noticia> noticias = new();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, Titulo, Descripcion, ImagenUrl, FechaPublicacion FROM Noticias";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    noticias.Add(new Noticia
                    {
                        Id = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        ImagenUrl = reader.GetString(3),
                        FechaPublicacion = reader.GetDateTime(4)
                    });
                }
            }
        }

        return noticias;
    }
}
