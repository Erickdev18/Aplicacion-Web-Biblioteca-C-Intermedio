using Biblio.Web.Result;

namespace Biblio.Web.DATA
{
    public interface IPrestamoDao
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Prestamo prestamo);
        Task<OperationResult> UpdateAsync(Prestamo prestamo);
    }
}
