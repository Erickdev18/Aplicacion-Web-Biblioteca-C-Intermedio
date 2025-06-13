using Biblio.Web.Exceptions;
using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace Biblio.Web.DATA
{
    public class CategoriaDao : ICategoriaDao
    {
        private readonly string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriaDao> _logger;

        public CategoriaDao(string connString,
                            IConfiguration configuration,
                            ILogger<CategoriaDao> logger)
        {
            _connString = connString;
            _configuration = configuration;
            _logger = logger;
        }
        public Task<OperationResult> GetAllAsync()
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                _logger.LogInformation("Obteniendo todas las categorias desde la base de datos.");
            }
            catch (Exception)
            {

                throw;
            }
            return Task.FromResult(Opresult);
        }
        public Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _logger.LogInformation($"Obteniendo la categoria con ID: {id} desde la base de datos.");
            }
            catch (Exception)
            {

                throw;
            }
            return Task.FromResult(Opresult);
        }
        public async Task<OperationResult> AddAsync(Categoria categoria, OperationResult operationResult)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                //if (categoria == null)

                //    Opresult = OperationResult.Failure("La categoria no puede ser nula.");

                //if (string.IsNullOrWhiteSpace(categoria!.Descripcion))
                //        Opresult = OperationResult.Failure("La descripcion de la categoria no puede ser nula o vacia.");


                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("Seguridad.GuardandoCategoria", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Descripcion", categoria!.Descripcion);
                        command.Parameters.AddWithValue("@p_Estado", categoria.Estado);
                        command.Parameters.AddWithValue("@p_FechaCreacion", categoria.FechaCreacion);
                        command.Parameters.AddWithValue("@p_UsuarioCreacionId", categoria.UsuarioCreacionId);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
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
                        command.Parameters.AddWithValue("@p_IdCategoria", categoria.CategoriaId);
                        command.Parameters.AddWithValue("@p_Descripcion", categoria.Descripcion);
                        command.Parameters.AddWithValue("@p_Estado", categoria.Estado);
                        command.Parameters.AddWithValue("@p_FechaMod", categoria.FechaMod);
                        command.Parameters.AddWithValue("@p_UsuarioModId", categoria.UsuarioModId);

                        SqlParameter p_Result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
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

        public Task<OperationResult> AddAsync(Categoria categoria)
        {
            throw new NotImplementedException();
        }
    }
}
