using HumanResources.Domain.Entities;
using HumanResources.Shared.Wrappers;

namespace HumanResources.Application.Interfaces
{
    public interface IEmployeeService : IGenericService<Employee>
    {
        Task<Response<bool>> DeleteEmployeeAsync(int id);
        Task<Response<bool>> InsertEmployeeAsync(Employee employee);
        Task<Response<bool>> UpdateEmployeeAsync(Employee employee);
    }
}
