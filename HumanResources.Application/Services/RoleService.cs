using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;

namespace HumanResources.Application.Services
{
    public class RoleService : GenericService<Role>, IRoleService
    {
        public RoleService(IGenericRepository<Role> repository) : base(repository)
        {
        }
    }
}
