using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;

namespace HumanResources.Application.Services
{
    public class CityService : GenericService<City>, ICityService
    {
        public CityService(IGenericRepository<City> repository) : base(repository)
        {
        }
    }
}
