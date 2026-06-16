using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;

namespace HumanResources.Application.Services
{
    public class EmployeeService : GenericService<Employee>, IEmployeeService
    {
        public EmployeeService(IGenericRepository<Employee> repository) : base(repository)
        {
        }
    }
}
