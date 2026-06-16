using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;

namespace HumanResources.Application.Services
{
    public class DepartmentService : GenericService<Department>, IDepartmentService
    {
        public DepartmentService(IGenericRepository<Department> repository) : base(repository)
        {
        }
    }
}
