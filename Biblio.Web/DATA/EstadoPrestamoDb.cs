using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Biblio.Web.Exceptions;

namespace Biblio.Web.DATA
{
    public class EstadoPrestamoDao : IEstadoPrestamo
    {
        private string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EstadoPrestamoDao> _logger;

        public EstadoPrestamoDao(IConfiguration configuration,
                                 ILogger<EstadoPrestamoDao> logger)
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
                    using (var command = new SqlCommand("Seguridad.ObtenerEstadoPrestamo", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<EstadoPrestamo> estadoPrestamos = new List<EstadoPrestamo>();
                            while (await reader.ReadAsync())
                            {
                                EstadoPrestamo estadoPrestamo = new EstadoPrestamo
                                {
                                    IdEstadoPrestamo = reader.GetInt32(0),
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
                                estadoPrestamos.Add(estadoPrestamo);
                            }
                            Opresult = OperationResult.Success("Estado Prestamo registrado correctamente.", estadoPrestamos);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontraron Estados Prestamos.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los Estado Prestamo de la base de datos.");
                Opresult = OperationResult.Failure("Error al obtener los estados de prestamos de la base de datos.");
            }
            return Opresult;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                _logger.LogInformation($"Obteniendo el EstadoPrestamo con ID: {id} de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerEstadoPrestamoPorID", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdEstadoPrestamo", id);
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();
                        if(reader.HasRows)
                        {
                            reader.Read();

                            EstadoPrestamo estadoPrestamo = new EstadoPrestamo();
                            {
                                estadoPrestamo.IdEstadoPrestamo = reader.GetInt32(0);
                                estadoPrestamo.Descripcion=reader.GetString(1);
                                estadoPrestamo.Estado = reader.GetBoolean(2);
                                estadoPrestamo.FechaCreacion = reader.GetDateTime(3);
                                estadoPrestamo.UsuarioCreacionId = reader.GetInt32(4);
                                estadoPrestamo.FechaMod = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
                                estadoPrestamo.UsuarioMod = reader.IsDBNull(6) ? null : reader.GetInt32(6);
                                estadoPrestamo.UsuarioElimino = reader.IsDBNull(7) ? null : reader.GetInt32(7);
                                estadoPrestamo.FechaElimino = reader.IsDBNull(8) ? null : reader.GetDateTime(8);
                                estadoPrestamo.Eliminado = reader.IsDBNull(9) ? null : reader.GetBoolean(9);
                            }
                            Opresult = OperationResult.Success("Estado Prestamo encontrado.", estadoPrestamo);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontro el Estado de Prestamo con el ID especificado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el Estado prestamo con ID {id} desde la base de datos.", ex.ToString());
                OperationResult.Failure($"Error al obtener el Estado prestamo con ID {id} desde la base de datos.");
            }
            return Opresult;
        }

        public async Task<OperationResult> AddAsync(EstadoPrestamo estadoPrestamo)
        {
            OperationResult Opresult = new OperationResult();
            
            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.GuardandoEstadoPrestamo", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Descripcion", estadoPrestamo.Descripcion);
                        command.Parameters.AddWithValue("@p_Estado", estadoPrestamo.Estado);
                        command.Parameters.AddWithValue("@p_FechaCreacion", estadoPrestamo.FechaCreacion);
                        command.Parameters.AddWithValue("@p_UsuarioCreacionId", estadoPrestamo.UsuarioCreacionId);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var result = (string)p_Result.Value;
                        if (result != "El Estado Prestamo fue creado correctamente.")
                            Opresult = OperationResult.Failure($"Error al agregar el Estado de Prestamo: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error agregando un Estado de prestamo {ex.Message}", ex.ToString());
            }
            return Opresult;
        }

        public async Task<OperationResult> UpdateAsync(EstadoPrestamo estadoPrestamo)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ActualizarEstadoPrestamo", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdEstadoPrestamo", estadoPrestamo.IdEstadoPrestamo);
                        command.Parameters.AddWithValue("@p_Descripcion", estadoPrestamo.Descripcion);
                        command.Parameters.AddWithValue("@p_Estado", estadoPrestamo.Estado);
                        command.Parameters.AddWithValue("@p_FechaMod", estadoPrestamo.FechaMod);
                        command.Parameters.AddWithValue("@p_UsuarioMod", estadoPrestamo.UsuarioMod);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var result = (string)p_Result.Value;
                        if (result != "El estado de préstamo fue actualizado correctamente.")
                            Opresult = OperationResult.Failure($"Error al actualizar el Estado de préstamo: {result}");
                        else
                            Opresult = OperationResult.Success(result);

                        Opresult = OperationResult.Success("Estado Prestamo actualizado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error actualizando el Estado de prestamo {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure("Error al actualizar el Estado de Prestamo.");
            }
            return Opresult;
        }
    }
}
