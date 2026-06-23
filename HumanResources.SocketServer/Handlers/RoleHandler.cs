using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Role;
using HumanResources.Shared.Wrappers;
using HumanResources.SocketServer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class RoleHandler
    {
        internal static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            IRoleService _roleService = scopedProvider.GetRequiredService<IRoleService>();

            try
            {
                switch (action)
                {
                    case 0:
                        RoleCreateDto? createDto = JsonSerializer.Deserialize<RoleCreateDto>(jsonPayload);

                        if (createDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para crear rol.");

                        Role newRole = new()
                        {
                            Name = createDto.Name,
                            MinSalary = createDto.MinSalary,
                            MaxSalary = createDto.MaxSalary
                        };

                        Response<bool> insertResponse = await _roleService.InsertAsync(newRole);
                        return JsonSerializer.Serialize(insertResponse);

                    case 1:
                        RoleUpdateDto? updateDto = JsonSerializer.Deserialize<RoleUpdateDto>(jsonPayload);

                        if (updateDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para actualizar rol.");

                        Role roleToUpdate = new()
                        {
                            Id = updateDto.Id,
                            Name = updateDto.Name,
                            MinSalary = updateDto.MinSalary,
                            MaxSalary = updateDto.MaxSalary
                        };

                        Response<bool> updateResponse = await _roleService.UpdateAsync(roleToUpdate);
                        return JsonSerializer.Serialize(updateResponse);

                    case 2:
                        Response<IEnumerable<Role>> responseList = await _roleService.GetAllAsync();

                        if (!responseList.IsSuccess)
                            return JsonSerializer.Serialize(Response<List<RoleResponseDto>>.Fail(responseList.Message));

                        List<RoleResponseDto> list = [.. responseList.Data!.Select(p => new RoleResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            MinSalary = p.MinSalary,
                            MaxSalary = p.MaxSalary
                        })];

                        return JsonSerializer.Serialize(Response<List<RoleResponseDto>>.Success(list, "Roles recuperados con éxito."));

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<Role> roleResponse = await _roleService.GetByIdAsync(searchedId);

                        if (!roleResponse.IsSuccess)
                            return JsonSerializer.Serialize(Response<RoleResponseDto>.Fail(roleResponse.Message));

                        RoleResponseDto responseDto = new()
                        {
                            Id = roleResponse.Data!.Id,
                            Name = roleResponse.Data.Name,
                            MinSalary = roleResponse.Data.MinSalary,
                            MaxSalary = roleResponse.Data.MaxSalary
                        };

                        return JsonSerializer.Serialize(Response<RoleResponseDto>.Success(responseDto));

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<bool> deleteResponse = await _roleService.DeleteAsync(deletedId);
                        return JsonSerializer.Serialize(deleteResponse);

                    default:
                        return SerializeHelper.SerializeFail("Acción no implementada.");
                }
            }
            catch (Exception ex)
            {
                return SerializeHelper.SerializeFail($"ERROR EN ROLE HANDLER: {ex.Message}");
            }
        }
    }
}
