using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.City;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    public class CityHandler
    {
        public static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            ICityService _cityService = scopedProvider.GetRequiredService<ICityService>();

            try
            {
                switch (action)
                {
                    case 0:
                        CityCreateDto? createDto = JsonSerializer.Deserialize<CityCreateDto>(jsonPayload);
                        if (createDto == null) return "ERROR: JSON inválido para crear ciudad.";

                        City newCity = new() { Name = createDto.Name };
                        bool inserted = await _cityService.InsertAsync(newCity);

                        return inserted ? "Ciudad creada exitosamente." : "ERROR: No se pudo crear la ciudad.";

                    case 2:
                        List<City> list = await _cityService.GetAllAsync();

                        List<CityResponseDto> responseList = [.. list.Select(p => new CityResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                        })];

                        return JsonSerializer.Serialize(responseList);

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        City? city = await _cityService.GetByIdAsync(searchedId);
                        if (city == null) return "ERROR: Ciudad no encontrada.";

                        CityResponseDto responseDto = new() { Id = city.Id, Name = city.Name };
                        return JsonSerializer.Serialize(new List<CityResponseDto> { responseDto });

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        bool deleted = await _cityService.DeleteAsync(deletedId);
                        return deleted ? "Registro borrado exitosamente." : "ERROR: No se pudo borrar o el ID no existe.";

                    default:
                        return "ERROR: Acción no implementada.";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR EN CITY HANDLER: {ex.Message}";
            }
        }
    }
}
