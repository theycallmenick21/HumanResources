using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Country;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    public static class CountryHandler
    {
        public static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            ICountryService _countryService = scopedProvider.GetRequiredService<ICountryService>();

            try
            {
                switch (action)
                {
                    case 0:
                        CountryCreateDto? createDto = JsonSerializer.Deserialize<CountryCreateDto>(jsonPayload);
                        if (createDto == null) return "ERROR: JSON inválido para crear país.";

                        Country newCountry = new() { Name = createDto.Name };
                        bool inserted = await _countryService.InsertAsync(newCountry);

                        return inserted ? "País creado exitosamente." : "ERROR: No se pudo crear el país.";

                    case 2:
                        List<Country> list = await _countryService.GetAllAsync();

                        List<CountryResponseDto> responseList = [.. list.Select(p => new CountryResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name
                        })];

                        return JsonSerializer.Serialize(responseList);

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Country? country = await _countryService.GetByIdAsync(searchedId);
                        if (country == null) return "ERROR: País no encontrado.";

                        CountryResponseDto responseDto = new() { Id = country.Id, Name = country.Name };
                        return JsonSerializer.Serialize(new List<CountryResponseDto> { responseDto });

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        bool deleted = await _countryService.DeleteAsync(deletedId);
                        return deleted ? "Registro borrado exitosamente." : "ERROR: No se pudo borrar o el ID no existe.";

                    default:
                        return "ERROR: Acción no implementada.";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR EN COUNTRY HANDLER: {ex.Message}";
            }
        }
    }
}