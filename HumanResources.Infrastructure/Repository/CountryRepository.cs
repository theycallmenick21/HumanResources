using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;
using HumanResources.Infrastructure.Data;

namespace HumanResources.Infrastructure.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(HumanResourcesContext context) : base(context)
        {
        }
    }
}
