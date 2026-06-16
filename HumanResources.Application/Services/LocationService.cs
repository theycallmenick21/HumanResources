using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;

namespace HumanResources.Application.Services
{
    public class LocationService : GenericService<Location>, ILocationService
    {
        public LocationService(IGenericRepository<Location> repository) : base(repository)
        {
        }
    }
}
