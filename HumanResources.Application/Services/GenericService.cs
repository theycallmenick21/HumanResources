using HumanResources.Application.Interfaces;
using HumanResources.Domain.Interfaces;

namespace HumanResources.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> InsertAsync(T entidad)
        {
            try
            {
                await _repository.InsertAsync(entidad);
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
                await _repository.UpdateAsync(entidad);
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
                await _repository.DeleteAsync(id);
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
                return await _repository.GetByIdAsync(id);
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
                var resultados = await _repository.GetAllAsync();
                return [.. resultados];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DB - ObtenerTodos {typeof(T).Name}]: {ex.Message}");
                return [];
            }
        }
    }
}