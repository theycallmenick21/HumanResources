using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Country;
using HumanResources.Shared.Wrappers;
using HumanResources.SocketServer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal static class CountryHandler
    {
        internal static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            ICountryService _countryService = scopedProvider.GetRequiredService<ICountryService>();

            try
            {
                switch (action)
                {
                    case 0:
                        CountryCreateDto? createDto = JsonSerializer.Deserialize<CountryCreateDto>(jsonPayload);

                        if (createDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para crear país.");

                        Country newCountry = new()
                        {
                            Name = createDto.Name
                        };

                        Response<bool> insertResponse = await _countryService.InsertAsync(newCountry);
                        return JsonSerializer.Serialize(insertResponse);

                    case 1:
                        CountryUpdateDto? updateDto = JsonSerializer.Deserialize<CountryUpdateDto>(jsonPayload);

                        if (updateDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para actualizar país.");

                        Country countryToUpdate = new()
                        {
                            Id = updateDto.Id,
                            Name = updateDto.Name
                        };

                        Response<bool> updateResponse = await _countryService.UpdateAsync(countryToUpdate);
                        return JsonSerializer.Serialize(updateResponse);

                    case 2:
                        Response<IEnumerable<Country>> responseList = await _countryService.GetAllAsync();

                        if (!responseList.IsSuccess)
                            return JsonSerializer.Serialize(Response<List<CountryResponseDto>>.Fail(responseList.Message));

                        List<CountryResponseDto> list = [.. responseList.Data!.Select(p => new CountryResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name
                        })];

                        return JsonSerializer.Serialize(Response<List<CountryResponseDto>>.Success(list, "Países recuperados con éxito."));

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<Country> cityResponse = await _countryService.GetByIdAsync(searchedId);

                        if (!cityResponse.IsSuccess)
                            return JsonSerializer.Serialize(Response<CountryResponseDto>.Fail(cityResponse.Message));

                        CountryResponseDto responseDto = new()
                        {
                            Id = cityResponse.Data!.Id,
                            Name = cityResponse.Data.Name
                        };

                        return JsonSerializer.Serialize(Response<CountryResponseDto>.Success(responseDto));

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<bool> deleteResponse = await _countryService.DeleteAsync(deletedId);
                        return JsonSerializer.Serialize(deleteResponse);

                    default:
                        return SerializeHelper.SerializeFail("Acción no implementada.");
                }
            }
            catch (Exception ex)
            {
                return SerializeHelper.SerializeFail($"ERROR EN COUNTRY HANDLER: {ex.Message}");
            }
        }
    }
}