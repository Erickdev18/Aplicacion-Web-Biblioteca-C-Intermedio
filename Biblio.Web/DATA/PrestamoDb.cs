using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Biblio.Web.Exceptions;

namespace Biblio.Web.DATA
{
    public class PrestamoDao : IPrestamoDao
    {
        private string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PrestamoDao> _logger;

        public PrestamoDao(IConfiguration configuration,
                          ILogger<PrestamoDao> logger)
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
                _logger.LogInformation("Obteniendo todos los Prestamos de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerPrestamo", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Prestamo> prestamos = new List<Prestamo>();
                            while (await reader.ReadAsync())
                            {
                                Prestamo prestamo = new Prestamo
                                {
                                    IdPrestamo = reader.GetInt32(0),
                                    Codigo = reader.GetString(1),
                                    IdEstadoPrestamo = reader.GetInt32(2),
                                    LibroID = reader.GetInt32(3),
                                    FechaDevolucion = reader.GetDateTime(4),
                                    FechaConfirmacionDevolucion = reader.GetDateTime(5),
                                    EstadoEntregado = reader.GetString(6),
                                    EstadoRecibido = reader.GetString(7),
                                    FechaCreacion = reader.GetDateTime(8),
                                    UsuarioCreacionId = reader.GetInt32(9),
                                    FechaMod = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                                    UsuarioMod = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                                    UsuarioElimino = reader.IsDBNull(12) ? null : reader.GetInt32(12),
                                    FechaElimino = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                                    Eliminado = reader.IsDBNull(14) ? null : reader.GetBoolean(14)


                                };
                                prestamos.Add(prestamo);
                            }
                            Opresult = OperationResult.Success("Prestamo registrado correctamente.", prestamos);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontraron Prestamos.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los prestamos desde la base de datos.");

                Opresult = OperationResult.Failure("Error al obtener los prestamos desde la base de datos.");
            }
            return Opresult;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();
            try
            {

                _logger.LogInformation($"Obteniendo el Prestamo con ID: {id} de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerLibroPorID", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdPrestamo", id);
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            Prestamo prestamo = new Prestamo();
                            {
                                prestamo.IdPrestamo = reader.GetInt32(0);
                                prestamo.Codigo = reader.GetString(1);
                                prestamo.IdEstadoPrestamo = reader.GetInt32(2);
                                prestamo.LibroID = reader.GetInt32(3);
                                prestamo.FechaDevolucion = reader.GetDateTime(4);
                                prestamo.FechaConfirmacionDevolucion = reader.GetDateTime(5);
                                prestamo.EstadoEntregado = reader.GetString(6);
                                prestamo.EstadoRecibido = reader.GetString(7);
                                prestamo.FechaCreacion = reader.GetDateTime(8);
                                prestamo.UsuarioCreacionId = reader.GetInt32(9);
                                prestamo.FechaMod = reader.IsDBNull(10) ? null : reader.GetDateTime(10);
                                prestamo.UsuarioMod = reader.IsDBNull(11) ? null : reader.GetInt32(11);
                                prestamo.UsuarioElimino = reader.IsDBNull(12) ? null : reader.GetInt32(12);
                                prestamo.FechaElimino = reader.IsDBNull(13) ? null : reader.GetDateTime(13);
                                prestamo.Eliminado = reader.IsDBNull(14) ? null : reader.GetBoolean(14);
                            }
                            Opresult = OperationResult.Success("Prestamo encontrado", prestamo);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontro el prestamo con el ID especificado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el prestamo con ID {id} desde la base de datos.", ex.ToString());
                OperationResult.Failure($"Error al obtener el prestamo con ID {id} desde la base de datos.");
            }
            return Opresult;
        }

        public async Task<OperationResult> AddAsync(Prestamo prestamo)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.GuardandoPrestamo", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Codigo", prestamo.Codigo);
                        command.Parameters.AddWithValue("@p_IdEstadoPrestamo", prestamo.IdEstadoPrestamo);
                        command.Parameters.AddWithValue("@p_IdLibro", prestamo.LibroID);
                        command.Parameters.AddWithValue("@p_FechaDevolucion", prestamo.FechaDevolucion);
                        command.Parameters.AddWithValue("@p_FechaConfirmacionDevolucion", prestamo.FechaConfirmacionDevolucion);
                        command.Parameters.AddWithValue("@p_EstadoEntregado", prestamo.EstadoEntregado);
                        command.Parameters.AddWithValue("@p_EstadoRecibido", prestamo.EstadoRecibido);
                        command.Parameters.AddWithValue("@p_UsuarioCreacionId", prestamo.UsuarioCreacionId);
                        command.Parameters.AddWithValue("@p_FechaCreacion", prestamo.FechaCreacion);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var result = (string)p_Result.Value;
                        if (result != "Préstamo registrado correctamente")
                            Opresult = OperationResult.Failure($"Error al agregar el Prestamo: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error agregando un libro {ex.Message}", ex.ToString());
            }
            return Opresult;
        }

        public async Task<OperationResult> UpdateAsync(Prestamo prestamo)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ActualizandoPrestamo", connection))
                    {
                        command.Parameters.AddWithValue("@p_IdPrestamo", prestamo.IdPrestamo);
                        command.Parameters.AddWithValue("@p_Codigo", prestamo.Codigo);
                        command.Parameters.AddWithValue("@p_IdEstadoPrestamo", prestamo.IdEstadoPrestamo);
                        command.Parameters.AddWithValue("@p_IdLibro", prestamo.LibroID);
                        command.Parameters.AddWithValue("@p_FechaDevolucion", prestamo.FechaDevolucion);
                        command.Parameters.AddWithValue("@p_FechaConfirmacionDevolucion", prestamo.FechaConfirmacionDevolucion);
                        command.Parameters.AddWithValue("@p_EstadoEntregado", prestamo.EstadoEntregado);
                        command.Parameters.AddWithValue("@p_EstadoRecibido", prestamo.EstadoRecibido);
                        command.Parameters.AddWithValue("@p_UsuarioMod", prestamo.UsuarioMod ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_FechaMod", prestamo.FechaMod ?? (object)DBNull.Value);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var result = (string)p_Result.Value;
                        if (result != "El préstamo fue actualizado correctamente.")
                            Opresult = OperationResult.Failure($"Error al actualizar el préstamo: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error actualizando un prestamo {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure("Error al actualizar el prestamo.");
            }
            return Opresult;
        }
    }
}
