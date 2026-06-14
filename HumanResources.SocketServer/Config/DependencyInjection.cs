using HumanResources.Application.Interfaces;
using HumanResources.Application.Services;
using HumanResources.Domain.Interfaces;
using HumanResources.Infrastructure.Data;
using HumanResources.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HumanResources.SocketServer.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddDbContext<HumanResourcesContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryService, CountryService>();

            return services;
        }
    }
}