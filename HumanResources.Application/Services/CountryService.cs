using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;

namespace HumanResources.Application.Services
{
    public class CountryService : GenericService<Country>, ICountryService
    {
    }
}