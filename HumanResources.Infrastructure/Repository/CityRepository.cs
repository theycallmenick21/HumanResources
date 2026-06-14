using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;
using HumanResources.Infrastructure.Data;

namespace HumanResources.Infrastructure.Repository
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(HumanResourcesContext context) : base(context)
        {
        }
    }
}
