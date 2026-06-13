using HumanResources.DataAccess;
using HumanResources.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Services.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        public async Task<bool> InsertAsync(T entidad)
        {
            try
            {
                using var context = new RecursosHumanosContext();
                await context.Set<T>().AddAsync(entidad);
                await context.SaveChangesAsync();
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
                using var context = new RecursosHumanosContext();
                
                context.Set<T>().Update(entidad);
                await context.SaveChangesAsync();
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
                using var context = new RecursosHumanosContext();
                
                var entidadDb = await context.Set<T>().FindAsync(id);

                if (entidadDb == null) return false;

                context.Set<T>().Remove(entidadDb);
                await context.SaveChangesAsync();
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
                using var context = new RecursosHumanosContext();
                return await context.Set<T>().FindAsync(id);
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
                using var context = new RecursosHumanosContext();
                return await context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DB - ObtenerTodos {typeof(T).Name}]: {ex.Message}");
                return [];
            }
        }
    }
}