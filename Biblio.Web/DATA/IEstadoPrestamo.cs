using Biblio.Web.Result;
namespace Biblio.Web.DATA
{
    public interface IEstadoPrestamo
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(EstadoPrestamo estadoPrestamo);
        Task<OperationResult> UpdateAsync(EstadoPrestamo estadoPrestamo);
    }
}
