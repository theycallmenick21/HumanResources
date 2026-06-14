using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;
using HumanResources.Infrastructure.Data;

namespace HumanResources.Infrastructure.Repository
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(HumanResourcesContext context) : base(context)
        {
        }
    }
}
