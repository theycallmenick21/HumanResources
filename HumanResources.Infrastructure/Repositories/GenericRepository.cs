using HumanResources.Domain.Interfaces;
using HumanResources.Infrastructure.Data;
using HumanResources.Shared.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly HumanResourcesContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(HumanResourcesContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<Response<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                List<T> data = await _dbSet.ToListAsync();
                return Response<IEnumerable<T>>.Success(data, "Registros obtenidos correctamente.");
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return Response<IEnumerable<T>>.Fail($"Error al obtener los registros: {errorMsg}");
            }
        }

        public async Task<Response<T>> GetByIdAsync(int id)
        {
            try
            {
                T? entity = await _dbSet.FindAsync(id);

                if (entity == null)
                    return Response<T>.Fail($"No se encontró ningún registro con el ID {id}.");

                return Response<T>.Success(entity, "Registro encontrado exitosamente.");
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return Response<T>.Fail($"Error al consultar la base de datos: {errorMsg}");
            }
        }

        public async Task<Response<bool>> InsertAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Response<bool>.Success(true, "Registro creado exitosamente.");
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return Response<bool>.Fail($"No se pudo crear el registro. Detalle: {errorMsg}");
            }
        }

        public async Task<Response<bool>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return Response<bool>.Success(true, "Registro actualizado correctamente.");
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return Response<bool>.Fail($"No se pudo actualizar el registro. Detalle: {errorMsg}");
            }
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            try
            {
                T? entidad = await _dbSet.FindAsync(id);

                if (entidad == null)
                    return Response<bool>.Fail($"No se puede borrar: El registro con ID {id} no existe.");

                _dbSet.Remove(entidad);
                await _context.SaveChangesAsync();
                return Response<bool>.Success(true, "Registro eliminado correctamente.");
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return Response<bool>.Fail($"No se pudo eliminar el registro. Es posible que esté relacionado con otras tablas. Detalle: {errorMsg}");
            }
        }
    }
}