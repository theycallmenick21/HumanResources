using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;
using HumanResources.Infrastructure.Data;

namespace HumanResources.Infrastructure.Repository
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(HumanResourcesContext context) : base(context)
        {
        }
    }
}
