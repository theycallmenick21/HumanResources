using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Location;
using HumanResources.Shared.Wrappers;
using HumanResources.SocketServer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class LocationHandler
    {
        internal static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            ILocationService _locationService = scopedProvider.GetRequiredService<ILocationService>();

            try
            {
                switch (action)
                {
                    case 0:
                        LocationCreateDto? createDto = JsonSerializer.Deserialize<LocationCreateDto>(jsonPayload);

                        if (createDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para crear ubicación.");

                        Location newEmployee = new()
                        {
                            Address = createDto.Address,
                            CityId = createDto.CityId
                        };

                        Response<bool> insertResponse = await _locationService.InsertAsync(newEmployee);
                        return JsonSerializer.Serialize(insertResponse);

                    case 1:
                        LocationUpdateDto? updateDto = JsonSerializer.Deserialize<LocationUpdateDto>(jsonPayload);

                        if (updateDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para actualizar ubicación.");

                        Location locationToUpdate = new()
                        {
                            Id = updateDto.Id,
                            Address = updateDto.Address,
                            CityId = updateDto.CityId
                        };

                        Response<bool> updateResponse = await _locationService.UpdateAsync(locationToUpdate);
                        return JsonSerializer.Serialize(updateResponse);

                    case 2:
                        Response<IEnumerable<Location>> responseList = await _locationService.GetAllAsync();

                        if (!responseList.IsSuccess)
                            return JsonSerializer.Serialize(Response<List<LocationResponseDto>>.Fail(responseList.Message));

                        List<LocationResponseDto> list = [.. responseList.Data!.Select(p => new LocationResponseDto
                        {
                            Id = p.Id,
                            Address = p.Address,
                            CityId = p.CityId
                        })];

                        return JsonSerializer.Serialize(Response<List<LocationResponseDto>>.Success(list, "Ubicaciones recuperadas con éxito."));

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<Location> locationResponse = await _locationService.GetByIdAsync(searchedId);

                        if (!locationResponse.IsSuccess)
                            return JsonSerializer.Serialize(Response<LocationResponseDto>.Fail(locationResponse.Message));

                        LocationResponseDto responseDto = new()
                        {
                            Id = locationResponse.Data!.Id,
                            Address = locationResponse.Data.Address,
                            CityId = locationResponse.Data.CityId
                        };

                        return JsonSerializer.Serialize(Response<LocationResponseDto>.Success(responseDto));

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<bool> deleteResponse = await _locationService.DeleteAsync(deletedId);
                        return JsonSerializer.Serialize(deleteResponse);

                    default:
                        return SerializeHelper.SerializeFail("Acción no implementada.");
                }
            }
            catch (Exception ex)
            {
                return SerializeHelper.SerializeFail($"ERROR EN LOCATION HANDLER: {ex.Message}");
            }
        }
    }
}
