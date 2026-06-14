using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;
using HumanResources.Infrastructure.Data;

namespace HumanResources.Infrastructure.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(HumanResourcesContext context) : base(context)
        {
        }
    }
}
