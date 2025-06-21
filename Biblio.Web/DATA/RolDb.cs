using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Biblio.Web.Exceptions;

namespace Biblio.Web.DATA
{
    public class RolDao : IRolDao
    {
        private string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RolDao> _logger;

        public RolDao(IConfiguration configuration, 
            ILogger<RolDao> logger)
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
                _logger.LogInformation("Iniciando la obtención de todos los roles desde la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerRol", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Rol> rols = new List<Rol>();
                            while (await reader.ReadAsync())
                            {
                                Rol rol = new Rol
                                {
                                    RoldId = reader.GetInt32(0),
                                    Nombre= reader. GetString(1),
                                    FechaCreacion = reader.GetDateTime(2),
                                    UsuarioCreacionId = reader.GetInt32(3),
                                    FechaMod = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                                    UsuarioMod = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                                    UsuarioElimino = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    FechaElimino = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                                    Eliminado = reader.IsDBNull(8) ? null : reader.GetBoolean(8)

                                };
                                rols.Add(rol);
                            }
                            Opresult = OperationResult.Success("Rol registrado correctamente.", rols);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontraron Roles");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los roles desde la base de datos.");

                Opresult = OperationResult.Failure("Error al obtener los roles desde la base de datos.");
            }
            return Opresult;
        }
        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _logger.LogInformation($"Obteniendo el Rol con ID: {id} de la base de datos");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerRolPorId", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RolId", id);
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            
                            Rol rol = new Rol();
                            {
                                rol.RoldId = reader.GetInt32(0);
                                rol.Nombre = reader.GetString(1);
                                rol.FechaCreacion = reader.GetDateTime(2);
                                rol.UsuarioCreacionId = reader.GetInt32(3);
                                rol.FechaMod = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                                rol.UsuarioMod = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                                rol.UsuarioElimino = reader.IsDBNull(6) ? null : reader.GetInt32(6);
                                rol.FechaElimino = reader.IsDBNull(7) ? null : reader.GetDateTime(7);
                                rol.Eliminado = reader.IsDBNull(8) ? null : reader.GetBoolean(8);
                            }
                            Opresult = OperationResult.Success("Rol encontrado correctamente.", rol);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontró el Rol con el ID especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el Rol con ID: {id} desde la base de datos.");
                Opresult = OperationResult.Failure("Error al obtener el Rol desde la base de datos.");
            }
            return Opresult;
        }
        public async Task<OperationResult> AddAsync(Rol rol)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.GuardandoRol", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", rol.Nombre);
                        command.Parameters.AddWithValue("@UsuarioCreacionId", rol.UsuarioCreacionId);
                        command.Parameters.AddWithValue("@FechaCreacion", rol.FechaCreacion);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var result = (string)p_Result.Value;
                        if (result != "Rol creado correctamente:")
                        {
                            Opresult = OperationResult.Failure($"Error al agregar el Rol: {result}");
                        }
                        else
                        {
                            Opresult = OperationResult.Success(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando un Rol {ex.Message}", ex.ToString());
            }
            return Opresult;
        }
        public async Task<OperationResult> UpdateAsync(Rol rol)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                 using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ActualizandoRol", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RolId", rol.RoldId);
                        command.Parameters.AddWithValue("@Nombre", rol.Nombre);
                        command.Parameters.AddWithValue("@UsuarioMod", rol.UsuarioMod);
                        command.Parameters.AddWithValue("@FechaMod", rol.FechaMod);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var result = (string)p_Result.Value;
                        if (result != "El rol fue actualizado correctamente.")
                        {
                            Opresult = OperationResult.Failure($"Error al actualizar el Rol: {result}");
                        }
                        else
                        {
                            Opresult = OperationResult.Success(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error actualizando un Rol {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure("Error al actualizar el Rol.");
            }
            return Opresult;
        }
    }

}
