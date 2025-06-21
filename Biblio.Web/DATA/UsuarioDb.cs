using Biblio.Web.Exceptions;
using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Biblio.Web.DATA
{
    public class UsuarioDao : IUsuarioDao
    {
        private string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuarioDao> _logger;

        public UsuarioDao(IConfiguration configuration,
                                 ILogger<UsuarioDao> logger)
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
                _logger.LogInformation("Obteniendo todos los Usuarios de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerUsuarios", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Usuario> usuarios = new List<Usuario>();
                            while (await reader.ReadAsync())
                            {
                                Usuario usuario = new Usuario
                                {

                                    UsuarioId = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2),
                                    Correo = reader.GetString(3),
                                    Clave = reader.GetString(4),
                                    Telefono = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    FechaCreacion = reader.GetDateTime(6),
                                    UsuarioCreacionId = reader.GetInt32(7),
                                    FechaMod = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                                    UsuarioMod = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                                    UsuarioElimino = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                                    FechaElimino = reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                                    Eliminado = reader.IsDBNull(12) ? (bool?)null : reader.GetBoolean(12),
                                    Estado = reader.IsDBNull(13) ? false : reader.GetBoolean(13)


                                };
                                usuarios.Add(usuario);
                            }
                            Opresult = OperationResult.Success("Usuario registrado correctamente.", usuarios);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los Usuarios de la base de datos.");
                Opresult = OperationResult.Failure("Error al obtener los usuarios de la base de datos.");
            }
            return Opresult;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _logger.LogInformation($"Obteniendo el Usuario con ID: {id} de la base de datos.");
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ObtenerUsuariosPorID", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_UsuarioId", id);
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            Usuario usuario = new Usuario();
                            {
                                usuario.UsuarioId = reader.GetInt32(0);
                                usuario.Nombre = reader.GetString(1);
                                usuario.Apellido = reader.GetString(2);
                                usuario.Correo = reader.GetString(3);
                                usuario.Clave = reader.GetString(4);
                                usuario.Telefono = reader.IsDBNull(5) ? null : reader.GetString(5);
                                usuario.FechaCreacion = reader.GetDateTime(6);
                                usuario.UsuarioCreacionId = reader.GetInt32(7);
                                usuario.FechaMod = reader.IsDBNull(8) ? null : reader.GetDateTime(8);
                                usuario.UsuarioMod = reader.IsDBNull(9) ? null : reader.GetInt32(9);
                                usuario.UsuarioElimino = reader.IsDBNull(10) ? null : reader.GetInt32(10);
                                usuario.FechaElimino = reader.IsDBNull(11) ? null : reader.GetDateTime(11);
                                usuario.Eliminado = reader.IsDBNull(12) ? (bool?)null : reader.GetBoolean(12);
                                usuario.Estado = reader.IsDBNull(13) ? false : reader.GetBoolean(13);
                            }
                            Opresult = OperationResult.Success("Usuario encontrado.", usuario);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No se encontro el Usuario con el ID especificado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el Usuario con ID {id} desde la base de datos.", ex.ToString());
                OperationResult.Failure($"Error al obtener el Usuario con ID {id} desde la base de datos.");
            }
            return Opresult;
        }
        public async Task<OperationResult> AddAsync(Usuario usuario)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.GuardandoUsuario", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@p_Apellido", usuario.Apellido ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_Correo", usuario.Correo);
                        command.Parameters.AddWithValue("@p_Clave", usuario.Clave);
                        command.Parameters.AddWithValue("@p_Telefono", usuario.Telefono ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_FechaCreacion", usuario.FechaCreacion);
                        command.Parameters.AddWithValue("@p_UsuarioCreacionId", usuario.UsuarioCreacionId);
                        command.Parameters.AddWithValue("@p_Estado", usuario.Estado);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var result = (string)p_Result.Value;
                        if (result != "Usuario creado correctamente: ")
                            Opresult = OperationResult.Failure($"Error al agregar el Usuario: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error agregando un Ususario {ex.Message}", ex.ToString());
            }
            return Opresult;
        }

        public async Task<OperationResult> UpdateAsync(Usuario usuario)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.ActualizandoUsuario", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_IdUsuario", usuario.UsuarioId);
                        command.Parameters.AddWithValue("@p_Nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@p_Apellido", usuario.Apellido);
                        command.Parameters.AddWithValue("@p_Correo", usuario.Correo);
                        command.Parameters.AddWithValue("@p_Clave", usuario.Clave);
                        command.Parameters.AddWithValue("@p_Telefono", usuario.Telefono ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@p_FechaMod", usuario.FechaMod);
                        command.Parameters.AddWithValue("@p_UsuarioMod", usuario.UsuarioMod);
                        command.Parameters.AddWithValue("@p_Estado", usuario.Estado);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_Result);
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var result = (string)p_Result.Value;
                        if (result != "El usuario fue actualizado correctamente.")
                            Opresult = OperationResult.Failure($"Error al actualizar el Usuario: {result}");
                        else
                            Opresult = OperationResult.Success(result);

                        Opresult = OperationResult.Success("Usuario actualizado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error actualizando el Usuario {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure("Error al actualizar el Usuario.");
            }
            return Opresult;
        }
    }
}
