namespace HumanResources.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<bool> InsertAsync(T entidad);
        Task<bool> UpdateAsync(T entidad);
        Task<bool> DeleteAsync(int id);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
    }
}