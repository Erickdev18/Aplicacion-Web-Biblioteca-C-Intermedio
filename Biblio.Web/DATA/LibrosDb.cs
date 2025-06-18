using Biblio.Web.Result;
using Microsoft.Data.SqlClient;

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
                            List<Libros> categorias = new List<Libros>();// se puede usar hashset<categoria> para evitar elementos duplicados
                            while (reader.Read())
                            {

                                Libros libros = new Libros
                                {
                                    LibroID = reader.GetInt32(0),
                                    Titulo = reader.GetString(1),
                                    Autor = reader.GetString(2),
                                    ISBN = reader.GetString(3),
                                    Ejemplares = reader.GetInt32(4),
                                    Editorial = reader.GetString(5),
                                    CategoriaId = reader.GetInt32(6),
                                    Estado= reader.GetBoolean (7),
                                    FechaCreacion =reader.GetDateTime(8),
                                    UsuarioCreacionId =reader.GetInt32(9),
                                    FechaMod = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                                    UsuarioMod = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                                    UsuarioElimino = reader.IsDBNull(12) ? null : reader.GetInt32(12),
                                    FechaElimino = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                                    Eliminado = reader.IsDBNull(14) ? null : reader.GetBoolean(14)

                                    //CategoriaId = reader.GetInt32(0),
                                    //Descripcion = reader.GetString(1),
                                    //Estado = reader.GetBoolean(2),
                                    //FechaCreacion = reader.GetDateTime(3),
                                    //UsuarioCreacionId = reader.GetInt32(4),
                                    //FechaMod = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                                    //UsuarioMod = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    //UsuarioElimino = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                                    //FechaElimino = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                                    //Eliminado = reader.IsDBNull(9) ? null : reader.GetBoolean(9)
                                };
                                categorias.Add(libros);
                            }
                            Opresult = OperationResult.Success("Categorias obtenidas correctamente.", categorias);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontraron categorias.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // estaba   _logger.LogError("Error al obtener las categorias desde la base de datos.", ex.ToString()); pero se quedaba con error, fue corregida con la siguiente manera:
                // _logger.LogError(ex, "Error al obtener las categorias desde la base de datos.");
                _logger.LogError(ex, "Error al obtener las categorias desde la base de datos.");

                Opresult = OperationResult.Failure("Error al obtener las categorias desde la base de datos.");
            }
            return Opresult;
        }

        public Task<OperationResult> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
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
                        command.Parameters.AddWithValue("@p_Titulo", libros.Titulo);
                        command.Parameters.AddWithValue("@p_Autor", libros.Autor);
                        command.Parameters.AddWithValue("@p_ISBN", libros.ISBN);
                        command.Parameters.AddWithValue("@p_Ejemplares", libros.Ejemplares ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_Editorial", libros.Editorial ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_CategoriaId", libros.CategoriaId);
                        command.Parameters.AddWithValue("@p_EstadoID", libros.Estado ?? (object)DBNull.Value);
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

        public Task<OperationResult> UpdateAsync(Libros libros)
        {
            throw new NotImplementedException();
        }
    }
}
