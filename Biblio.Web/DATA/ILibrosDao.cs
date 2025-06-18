using Biblio.Web.Result;

namespace Biblio.Web.DATA
{
    public interface ILibrosDao
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Libros libros);

        Task<OperationResult> UpdateAsync(Libros libros);
    }
}
