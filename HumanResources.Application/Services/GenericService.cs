using HumanResources.Application.Interfaces;
using HumanResources.Domain.Interfaces;
using HumanResources.Shared.Wrappers;

namespace HumanResources.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository) => _repository = repository;

        public async Task<Response<bool>> InsertAsync(T entidad) => await _repository.InsertAsync(entidad);

        public async Task<Response<bool>> UpdateAsync(T entidad) => await _repository.UpdateAsync(entidad);

        public async Task<Response<bool>> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<Response<T>> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<Response<IEnumerable<T>>> GetAllAsync() => await _repository.GetAllAsync();
    }
}