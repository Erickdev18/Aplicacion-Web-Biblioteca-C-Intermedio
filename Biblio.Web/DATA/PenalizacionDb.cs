using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Biblio.Web.Exceptions;

namespace Biblio.Web.DATA
{
    public class PenalizacionDao : IPenalizacionDao
    {

        private string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PenalizacionDao> _logger;

        public PenalizacionDao(IConfiguration configuration,
                                 ILogger<PenalizacionDao> logger)
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
                _logger.LogInformation("Obteniendo todos las Penalizaciones de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerPenalizacion", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Penalizacion> penalizacions = new List<Penalizacion>();
                            while (await reader.ReadAsync())
                            {
                                Penalizacion penalizacion = new Penalizacion
                                {
                                    IdPenalizacion = reader.GetInt32(0),
                                    IdPrestamo = reader.GetInt32(1),
                                    IdUsuario = reader.GetInt32(2),
                                    IdLibro = reader.GetInt32(3),
                                    Fecha_Retraso = reader.GetDateTime(4),
                                    Dias_Retraso = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                                    Monto_Penalizacion = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Estado_Penalizacion = reader.GetBoolean(7),
                                    Fecha_Pago = reader.GetDateTime(8),
                                    Metodo_Pago = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    Descripcion = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    FechaCreacion = reader.GetDateTime(11),
                                    UsuarioCreacionId = reader.GetInt32(12),
                                    FechaMod = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                                    UsuarioMod = reader.IsDBNull(14) ? null : reader.GetInt32(14),
                                    UsuarioElimino = reader.IsDBNull(15) ? null : reader.GetInt32(15),
                                    FechaElimino = reader.IsDBNull(16) ? null : reader.GetDateTime(16),
                                    Eliminado = reader.IsDBNull(17) ? false : reader.GetBoolean(17)
                                };
                                penalizacions.Add(penalizacion);
                            }
                            Opresult = OperationResult.Success("Penalizacion registrada correctamente.", penalizacions);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las Penalizaciones de la base de datos.");
                Opresult = OperationResult.Failure("Error al obtener las penalizaciones de la base de datos.");
            }
            return Opresult;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                _logger.LogInformation($"Obteniendo la Penalizacion con ID: {id} de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerPenalizacionPorID", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdPenalizacion", id);
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            Penalizacion penalizacion = new Penalizacion();
                            {
                                penalizacion.IdPenalizacion = reader.GetInt32(0);
                                penalizacion.IdPrestamo = reader.GetInt32(1);
                                penalizacion.IdUsuario = reader.GetInt32(2);
                                penalizacion.IdLibro = reader.GetInt32(3);
                                penalizacion.Fecha_Retraso = reader.GetDateTime(4);
                                penalizacion.Dias_Retraso = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                                penalizacion.Monto_Penalizacion = reader.IsDBNull(6) ? null : reader.GetInt32(6);
                                penalizacion.Estado_Penalizacion = reader.GetBoolean(7);
                                penalizacion.Fecha_Pago = reader.GetDateTime(8);
                                penalizacion.Metodo_Pago = reader.IsDBNull(9) ? null : reader.GetString(9);
                                penalizacion.Descripcion = reader.IsDBNull(10) ? null : reader.GetString(10);
                                penalizacion.FechaCreacion = reader.GetDateTime(11);
                                penalizacion.UsuarioCreacionId = reader.GetInt32(12);
                                penalizacion.FechaMod = reader.IsDBNull(13) ? null : reader.GetDateTime(13);
                                penalizacion.UsuarioMod = reader.IsDBNull(14) ? null : reader.GetInt32(14);
                                penalizacion.UsuarioElimino = reader.IsDBNull(15) ? null : reader.GetInt32(15);
                                penalizacion.FechaElimino = reader.IsDBNull(16) ? null : reader.GetDateTime(16);
                                penalizacion.Eliminado = reader.IsDBNull(17) ? false : reader.GetBoolean(17);
                            }
                            Opresult = OperationResult.Success("Penalizacion encontrado.", penalizacion);

                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontro la Penalizacion con el ID especificado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener la Penalidad con ID {id} desde la base de datos.", ex.ToString());
                OperationResult.Failure($"Error al obtener la Penalidad con ID {id} desde la base de datos.");
            }
            return Opresult;
        }

        public async Task<OperationResult> AddAsync(Penalizacion penalizacion)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.GuardandoPenalizacion", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdPrestamo", penalizacion.IdPrestamo);
                        command.Parameters.AddWithValue("@p_IdUsuario", penalizacion.IdUsuario);
                        command.Parameters.AddWithValue("@p_IdLibro", penalizacion.IdLibro);
                        command.Parameters.AddWithValue("@p_Fecha_Retraso", penalizacion.Fecha_Retraso);
                        command.Parameters.AddWithValue("@p_Dias_Retraso", penalizacion.Dias_Retraso);
                        command.Parameters.AddWithValue("@p_Monto_Penalizacion", penalizacion.Monto_Penalizacion);
                        command.Parameters.AddWithValue("@p_Estado_Penalizacion", penalizacion.Estado_Penalizacion);
                        command.Parameters.AddWithValue("@p_Fecha_Pago", penalizacion.Fecha_Pago);
                        command.Parameters.AddWithValue("@p_Metodo_Pago", penalizacion.Metodo_Pago);
                        command.Parameters.AddWithValue("@p_Descripcion", penalizacion.Descripcion);
                        command.Parameters.AddWithValue("@p_FechaCreacion", penalizacion.FechaCreacion);
                        command.Parameters.AddWithValue("@p_UsuarioCreacionId", penalizacion.UsuarioCreacionId);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var result = (string)p_Result.Value;
                        if (result != "Penalización registrada correctamente")
                            Opresult = OperationResult.Failure($"Error al agregar el Penalidad: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error agregando una Penalización {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure($"Error al agregar la Penalización: {ex.Message}");
            }
            return Opresult;
        }

        public async Task<OperationResult> UpdateAsync(Penalizacion penalizacion)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ActualizandoPenalizacion", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdPenalizacion", penalizacion.IdPenalizacion);
                        command.Parameters.AddWithValue("@p_IdPrestamo", penalizacion.IdPrestamo);
                        command.Parameters.AddWithValue("@p_IdUsuario", penalizacion.IdUsuario);
                        command.Parameters.AddWithValue("@p_IdLibro", penalizacion.IdLibro);
                        command.Parameters.AddWithValue("@p_Fecha_Retraso", penalizacion.Fecha_Retraso);
                        command.Parameters.AddWithValue("@p_Dias_Retraso", penalizacion.Dias_Retraso);
                        command.Parameters.AddWithValue("@p_Monto_Penalizacion", penalizacion.Monto_Penalizacion);
                        command.Parameters.AddWithValue("@p_Estado_Penalizacion", penalizacion.Estado_Penalizacion);
                        command.Parameters.AddWithValue("@p_Fecha_Pago", penalizacion.Fecha_Pago);
                        command.Parameters.AddWithValue("@p_Metodo_Pago", penalizacion.Metodo_Pago);
                        command.Parameters.AddWithValue("@p_Descripcion", penalizacion.Descripcion);
                        command.Parameters.AddWithValue("@p_UsuarioMod", penalizacion.UsuarioMod);
                        command.Parameters.AddWithValue("@p_FechaMod", penalizacion.FechaMod);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var result = (string)p_Result.Value;
                        if (result != "La penalización fue actualizada correctamente.")
                            Opresult = OperationResult.Failure($"Error al actualizar la Penalizacion: {result}");
                        else
                            Opresult = OperationResult.Success(result);

                        Opresult = OperationResult.Success("Penalizacion actualizado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error actualizando la Penalizacion {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure("Error al actualizar la Penalizacion.");
            }
            return Opresult;
        }
    }
}
