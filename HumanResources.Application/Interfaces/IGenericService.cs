using HumanResources.Shared.Wrappers;

namespace HumanResources.Application.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<Response<bool>> InsertAsync(T entidad);
        Task<Response<bool>> UpdateAsync(T entidad);
        Task<Response<bool>> DeleteAsync(int id);
        Task<Response<T>> GetByIdAsync(int id);
        Task<Response<IEnumerable<T>>> GetAllAsync();
    }
}