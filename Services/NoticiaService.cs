using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MedicinaESE.Models;

namespace MedicinaESE.Services;

// Servicio para gestionar noticias obtenidas desde SQL Server
public class NoticiaService
{
    private readonly string _connectionString;

    // Constructor: Recibe la cadena de conexión desde `Program.cs`
    public NoticiaService(string connectionString)
    {
        _connectionString = connectionString 
                            ?? throw new ArgumentNullException(nameof(connectionString)); // Evita valores `null`
    }

    // Método para obtener la lista de noticias desde la base de datos
    public List<Noticia> ObtenerNoticias()
    {
        List<Noticia> noticias = new();

        try
        {
            // Establecer la conexión con SQL Server
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open(); // Intenta conectar con la base de datos
                
                string query = "SELECT Id, Titulo, Descripcion, ImagenUrl, FechaPublicacion FROM Noticias"; // Consulta SQL

                // Ejecutar la consulta
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) // Leer los resultados fila por fila
                    {
                        noticias.Add(new Noticia
                        {
                            Id = reader.GetInt32(0), // ID de la noticia
                            Titulo = reader.GetString(1), // Título de la noticia
                            Descripcion = reader.GetString(2), // Descripción corta
                            ImagenUrl = reader.GetString(3), // URL de la imagen asociada
                            FechaPublicacion = reader.GetDateTime(4) // Fecha de publicación
                        });
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Si la base de datos no está disponible, lanzar un mensaje más claro
            throw new Exception($"Error de conexión con la base de datos: {ex.Message}");
        }

        return noticias; // Retorna la lista de noticias obtenidas
    }
}
