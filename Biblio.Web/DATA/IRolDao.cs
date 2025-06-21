using Biblio.Web.Result;

namespace Biblio.Web.DATA
{
    public interface IRolDao
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Rol rol);
        Task<OperationResult> UpdateAsync(Rol rol);
    }
}
