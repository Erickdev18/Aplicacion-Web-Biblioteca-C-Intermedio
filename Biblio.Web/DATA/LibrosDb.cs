using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Biblio.Web.Exceptions;

namespace Biblio.Web.DATA
{
    public class LibrosDao : ILibrosDao
    {
        private string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LibrosDao> _logger;

        public LibrosDao(
                    IConfiguration configuration,
                    ILogger<LibrosDao> logger)
        {

            _connString = configuration.GetConnectionString("biblioConn")
               ?? throw new InvalidOperationException("La cadena de conexión 'biblioConn' no está configurada.");

            _configuration = configuration;
            _logger = logger;
        }
        public async Task<OperationResult> GetAllAsync()
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _logger.LogInformation("Obteniendo todos los libros de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerLibros", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        await connection.OpenAsync();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Libros> libros = new List<Libros>();// se puede usar hashset<categoria> para evitar elementos duplicados
                            while (reader.Read())
                            {

                                Libros libro = new Libros
                                {
                                    LibroID = reader.GetInt32(0),
                                    Titulo = reader.GetString(1),
                                    Autor = reader.GetString(2),
                                    ISBN = reader.GetString(3),
                                    Ejemplares = reader.GetInt32(4),
                                    Editorial = reader.GetString(5),
                                    IdCategoria = reader.GetInt32(6),
                                    Estado = reader.GetBoolean(7),
                                    FechaCreacion = reader.GetDateTime(8),
                                    UsuarioCreacionId = reader.GetInt32(9),
                                    FechaMod = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                                    UsuarioMod = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                                    UsuarioElimino = reader.IsDBNull(12) ? null : reader.GetInt32(12),
                                    FechaElimino = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                                    Eliminado = reader.IsDBNull(14) ? null : reader.GetBoolean(14)

                                };
                                libros.Add(libro);
                            }
                            Opresult = OperationResult.Success("Libro Agregado correctamente.", libros);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontraron Libros.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error al obtener los libros desde la base de datos.");

                Opresult = OperationResult.Failure("Error al obtener los libros desde la base de datos.");
            }
            return Opresult;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _logger.LogInformation($"Obteniendo el libro con ID: {id} de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerLibroPorID", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_LibroID", id);
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            Libros libro = new Libros();
                            {
                                libro.LibroID = reader.GetInt32(0);
                                libro.Titulo = reader.GetString(1);
                                libro.Autor = reader.GetString(2);
                                libro.ISBN = reader.GetString(3);
                                libro.Ejemplares = reader.GetInt32(4);
                                libro.Editorial = reader.GetString(5);
                                libro.IdCategoria = reader.GetInt32(6);
                                libro.Estado = reader.GetBoolean(7);
                                libro.FechaCreacion = reader.GetDateTime(8);
                                libro.UsuarioCreacionId = reader.GetInt32(9);
                                libro.FechaMod = reader.IsDBNull(10) ? null : reader.GetDateTime(10);
                                libro.UsuarioMod = reader.IsDBNull(11) ? null : reader.GetInt32(11);
                                libro.UsuarioElimino = reader.IsDBNull(12) ? null : reader.GetInt32(12);
                                libro.FechaElimino = reader.IsDBNull(13) ? null : reader.GetDateTime(13);
                                libro.Eliminado = reader.IsDBNull(14) ? null : reader.GetBoolean(14);
                            }
                            ;

                            Opresult = OperationResult.Success("Libro encontrado.", libro);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontró el libro con el ID especificado.");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener la categoria con ID {id} desde la base de datos.", ex.ToString());
                OperationResult.Failure($"Error al obtener la categoria con ID {id} desde la base de datos.");

            }
            return Opresult;
        }

        public async Task<OperationResult> AddAsync(Libros libros)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.GuardandoLibro", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdLibro", libros.LibroID);
                        command.Parameters.AddWithValue("@p_Titulo", libros.Titulo);
                        command.Parameters.AddWithValue("@p_Autor", libros.Autor);
                        command.Parameters.AddWithValue("@p_ISBN", libros.ISBN);
                        command.Parameters.AddWithValue("@p_Ejemplares", libros.Ejemplares ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_Editorial", libros.Editorial ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_CategoriaId", libros.IdCategoria);
                        command.Parameters.AddWithValue("@p_EstadoID", libros.Estado);
                        command.Parameters.AddWithValue("@p_UsuarioCreacionId", libros.UsuarioCreacionId);
                        command.Parameters.AddWithValue("@p_FechaCreacion", libros.FechaCreacion);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_Result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var result = (string)p_Result.Value;

                        if (result != "El libro fue registrado correctamente.")

                            Opresult = OperationResult.Failure($"Error al agregar el libro: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error agregando un libro {ex.Message}", ex.ToString());
            }

            return Opresult;

        }

        public async Task<OperationResult> UpdateAsync(Libros libros)
        {
            OperationResult Opresult = new OperationResult();
            try
            {

                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ActualizandoLibro", connection))
                    {
                        command.Parameters.AddWithValue("@p_IdLibro", libros.LibroID);
                        command.Parameters.AddWithValue("@p_Titulo", libros.Titulo);
                        command.Parameters.AddWithValue("@p_Autor", libros.Autor);
                        command.Parameters.AddWithValue("@p_ISBN", libros.ISBN);
                        command.Parameters.AddWithValue("@p_Ejemplares", libros.Ejemplares ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_Editorial", libros.Editorial ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_CategoriaId", libros.IdCategoria);
                        command.Parameters.AddWithValue("@p_EstadoID", libros.Estado);
                        command.Parameters.AddWithValue("@p_UsuarioMod", libros.UsuarioMod ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_FechaMod", libros.FechaMod ?? (object)DBNull.Value);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_Result);
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var result = (string)p_Result.Value;
                        if (result != "El libro fue actualizado correctamente.")
                            Opresult = OperationResult.Failure($"Error al actualizar el libro: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error actualizando un libro {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure("Error al actualizar el libro.");
            }
            return Opresult;
        }
    }
} // Fin de la clase LibrosDao
