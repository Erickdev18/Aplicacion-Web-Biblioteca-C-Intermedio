using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Biblio.Web.Exceptions;

namespace Biblio.Web.DATA
{
    public class CategoriaDao : ICategoriaDao
    {
        //private readonly string _connString;
        private string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriaDao> _logger;

        public CategoriaDao(/*string connString,*/
                            IConfiguration configuration,
                            ILogger<CategoriaDao> logger)
        {
            //_connString = connString;
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
                _logger.LogInformation("Obteniendo todas las categorias desde la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerCategoria", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        await connection.OpenAsync();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Categoria> categorias = new List<Categoria>();// se puede usar hashset<categoria> para evitar elementos duplicados
                            while (reader.Read())
                            {
                                Categoria categoria = new Categoria
                                {
                                    IdCategoria = reader.GetInt32(0),
                                    Descripcion = reader.GetString(1),
                                    Estado = reader.GetBoolean(2),
                                    FechaCreacion = reader.GetDateTime(3),
                                    UsuarioCreacionId = reader.GetInt32(4),
                                    FechaMod = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                                    UsuarioMod = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    UsuarioElimino = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                                    FechaElimino = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                                    Eliminado = reader.IsDBNull(9) ? null : reader.GetBoolean(9)
                                };
                                categorias.Add(categoria);
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
        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _logger.LogInformation($"Obteniendo la categoria con ID: {id} desde la base de datos.");

                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerCategoriaPorId", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdCategoria", id);
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Categoria categoria = new Categoria();
                            {
                                categoria.IdCategoria = reader.GetInt32(0);
                                categoria.Descripcion = reader.GetString(1);
                                categoria.Estado = reader.GetBoolean(2);
                                categoria.FechaCreacion = reader.GetDateTime(3);
                                categoria.UsuarioCreacionId = reader.GetInt32(4);
                                categoria.FechaMod = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
                                categoria.UsuarioMod = reader.IsDBNull(6) ? null : reader.GetInt32(6);
                                categoria.UsuarioElimino = reader.IsDBNull(7) ? null : reader.GetInt32(7);
                                categoria.FechaElimino = reader.IsDBNull(8) ? null : reader.GetDateTime(8);
                                categoria.Eliminado = reader.IsDBNull(9) ? null : reader.GetBoolean(9);
                            }
                            Opresult = OperationResult.Success("Categoria obtenida correctamente.", categoria);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure($"No se encontró la categoria por ID {id}.");
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
        public async Task<OperationResult> AddAsync(Categoria categoria)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                //if (categoria == null)
                //    return OperationResult.Failure("La categoría no puede ser nula.");
                //if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                //    return OperationResult.Failure("La descripción es obligatoria.");
                //if (categoria.FechaCreacion == default)
                //    return OperationResult.Failure("La fecha de creación es obligatoria.");
                //if (categoria.UsuarioCreacionId == 0)
                //    return OperationResult.Failure("El usuario de creación es obligatorio.");

                //_logger.LogInformation($"Insertando: Descripcion={categoria.Descripcion}, Estado={categoria.Estado}, FechaCreacion={categoria.FechaCreacion}, UsuarioCreacionId={categoria.UsuarioCreacionId}");

                if (categoria == null)

                    Opresult = OperationResult.Failure("La categoria no puede ser nula.");

                if (string.IsNullOrWhiteSpace(categoria!.Descripcion))
                    Opresult = OperationResult.Failure("La descripcion de la categoria no puede ser nula o vacia.");


                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.GuardandoCategoria", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Descripcion", categoria!.Descripcion);
                        command.Parameters.AddWithValue("@p_Estado", categoria.Estado);
                        command.Parameters.AddWithValue("@p_FechaCreacion", categoria.FechaCreacion);
                        command.Parameters.AddWithValue("@p_UsuarioCreacionId", categoria.UsuarioCreacionId);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)//Estaba dando error porque era de tipo int
                        {
                            Size= 4000, // Asegúrate de que el tamaño sea suficiente para el mensaje de error
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_Result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var result = (string)p_Result.Value;

                        if (result != "Ok")

                            Opresult = OperationResult.Failure($"Error al agregar la categoria: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error agregando una categoria {ex.Message}", ex.ToString());
            }

            return Opresult;




        }
        public async Task<OperationResult> UpdateAsync(Categoria categoria)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                if (categoria == null)

                    Opresult = OperationResult.Failure("La categoria no puede ser nula.");

                if (string.IsNullOrWhiteSpace(categoria!.Descripcion))
                    Opresult = OperationResult.Failure("La descripcion de la categoria no puede ser nula o vacia.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ActualizandoCategoria", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdCategoria", categoria.IdCategoria);
                        command.Parameters.AddWithValue("@p_Descripcion", categoria.Descripcion);
                        command.Parameters.AddWithValue("@p_Estado", categoria.Estado);
                        command.Parameters.AddWithValue("@p_FechaMod", categoria.FechaMod);
                        command.Parameters.AddWithValue("@p_UsuarioMod", categoria.UsuarioMod);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000, // Asegúrate de que el tamaño sea suficiente para el mensaje de error
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_Result);
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var result = (string)p_Result.Value;
                        if (result != "Ok")
                            Opresult = OperationResult.Failure($"Error al actualizar la categoria: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error actualizando la categoria {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure($"Error actualizando la categoria: {ex.Message}");
            }
            return Opresult;
        }

    }
}
