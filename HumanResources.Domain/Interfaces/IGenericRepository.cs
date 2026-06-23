
using HumanResources.Shared.Wrappers;

namespace HumanResources.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<Response<IEnumerable<T>>> GetAllAsync();
        Task<Response<T>> GetByIdAsync(int id);
        Task<Response<bool>> InsertAsync(T entidad);
        Task<Response<bool>> UpdateAsync(T entidad);
        Task<Response<bool>> DeleteAsync(int id);
    }
}