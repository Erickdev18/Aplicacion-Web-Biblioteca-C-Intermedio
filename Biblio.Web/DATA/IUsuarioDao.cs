using Biblio.Web.Result;

namespace Biblio.Web.DATA
{
    public interface IUsuarioDao
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Usuario usuario);
        Task<OperationResult> UpdateAsync(Usuario usuario);
    }
}
