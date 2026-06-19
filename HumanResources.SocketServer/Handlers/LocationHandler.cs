using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Location;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class LocationHandler
    {
        public static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            ILocationService _locationService = scopedProvider.GetRequiredService<ILocationService>();

            try
            {
                switch (action)
                {
                    case 0:
                        LocationCreateDto? createDto = JsonSerializer.Deserialize<LocationCreateDto>(jsonPayload);
                        if (createDto == null) return "ERROR: JSON inválido para crear ubicación.";

                        Location newLocation = new()
                        {
                            Address = createDto.Address,
                            CityId = createDto.CityId
                        };

                        bool inserted = await _locationService.InsertAsync(newLocation);

                        return inserted ? "Ubicación creada exitosamente." : "ERROR: No se pudo crear la ubicación.";

                    case 2:
                        List<Location> list = await _locationService.GetAllAsync();

                        List<LocationResponseDto> responseList = [.. list.Select(p => new LocationResponseDto
                        {
                            Address = p.Address,
                            CityId = p.CityId
                        })];

                        return JsonSerializer.Serialize(responseList);

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int idBuscar = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Location? location = await _locationService.GetByIdAsync(idBuscar);
                        if (location == null) return "ERROR: Ubicación no encontrada.";

                        LocationResponseDto responseDto = new()
                        {
                            Id = location.Id,
                            Address = location.Address,
                            CityId = location.CityId
                        };

                        return JsonSerializer.Serialize(new List<LocationResponseDto> { responseDto });

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        bool deleted = await _locationService.DeleteAsync(deletedId);
                        return deleted ? "Registro borrado exitosamente." : "ERROR: No se pudo borrar o el ID no existe.";

                    default:
                        return "ERROR: Acción no implementada.";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR EN LOCATION HANDLER: {ex.Message}";
            }
        }
    }
}
