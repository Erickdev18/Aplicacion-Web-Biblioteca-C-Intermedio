using Biblio.Web.Result;

namespace Biblio.Web.DATA
{
    public interface IPenalizacionDao
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Penalizacion penalizacion);
        Task<OperationResult> UpdateAsync(Penalizacion penalizacion);
    }
}
