using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.City;
using HumanResources.Shared.Wrappers;
using HumanResources.SocketServer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class CityHandler
    {
        internal static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            ICityService _cityService = scopedProvider.GetRequiredService<ICityService>();

            try
            {
                switch (action)
                {
                    case 0:
                        CityCreateDto? createDto = JsonSerializer.Deserialize<CityCreateDto>(jsonPayload);

                        if (createDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para crear ciudad.");

                        City newCity = new()
                        {
                            Name = createDto.Name,
                            CountryId = createDto.CountryId
                        };

                        Response<bool> insertResponse = await _cityService.InsertAsync(newCity);
                        return JsonSerializer.Serialize(insertResponse);

                    case 1:
                        CityUpdateDto? updateDto = JsonSerializer.Deserialize<CityUpdateDto>(jsonPayload);

                        if (updateDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para actualizar ciudad.");

                        City cityToUpdate = new()
                        {
                            Id = updateDto.Id,
                            Name = updateDto.Name,
                            CountryId = updateDto.CountryId
                        };

                        Response<bool> updateResponse = await _cityService.UpdateAsync(cityToUpdate);
                        return JsonSerializer.Serialize(updateResponse);

                    case 2:
                        Response<IEnumerable<City>> responseList = await _cityService.GetAllAsync();

                        if (!responseList.IsSuccess)
                            return JsonSerializer.Serialize(Response<List<CityResponseDto>>.Fail(responseList.Message));

                        List<CityResponseDto> list = [.. responseList.Data!.Select(p => new CityResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            CountryId = p.CountryId
                        })];

                        return JsonSerializer.Serialize(Response<List<CityResponseDto>>.Success(list, "Ciudades recuperadas con éxito."));

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<City> cityResponse = await _cityService.GetByIdAsync(searchedId);

                        if (!cityResponse.IsSuccess)
                            return JsonSerializer.Serialize(Response<CityResponseDto>.Fail(cityResponse.Message));

                        CityResponseDto responseDto = new()
                        {
                            Id = cityResponse.Data!.Id,
                            Name = cityResponse.Data.Name,
                            CountryId = cityResponse.Data.CountryId
                        };

                        return JsonSerializer.Serialize(Response<CityResponseDto>.Success(responseDto));

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<bool> deleteResponse = await _cityService.DeleteAsync(deletedId);
                        return JsonSerializer.Serialize(deleteResponse);

                    default:
                        return SerializeHelper.SerializeFail("Acción no implementada.");
                }
            }
            catch (Exception ex)
            {
                return SerializeHelper.SerializeFail($"ERROR EN CITY HANDLER: {ex.Message}");
            }
        }
    }
}
