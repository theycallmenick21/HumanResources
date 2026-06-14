using HumanResources.Application.Interfaces;
using HumanResources.Infrastructure.Data;
using HumanResources.Infrastructure.Repositories;

namespace HumanResources.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        public async Task<bool> InsertAsync(T entidad)
        {
            try
            {
                await new GenericRepository<T>(new HumanResourcesContext()).InsertAsync(entidad);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DB - Insertar {typeof(T).Name}]: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T entidad)
        {
            try
            {
                await new GenericRepository<T>(new HumanResourcesContext()).UpdateAsync(entidad);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DB - Actualizar {typeof(T).Name}]: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await new GenericRepository<T>(new HumanResourcesContext()).DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DB - Borrar {typeof(T).Name}]: {ex.Message}");
                return false;
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await new GenericRepository<T>(new HumanResourcesContext()).GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DB - ObtenerPorId {typeof(T).Name}]: {ex.Message}");
                return null;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return (List<T>)await new GenericRepository<T>(new HumanResourcesContext()).GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DB - ObtenerTodos {typeof(T).Name}]: {ex.Message}");
                return [];
            }
        }
    }
}
