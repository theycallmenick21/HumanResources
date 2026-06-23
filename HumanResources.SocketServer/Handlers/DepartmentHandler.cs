using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Department;
using HumanResources.Shared.Wrappers;
using HumanResources.SocketServer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class DepartmentHandler
    {
        internal static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            IDepartmentService _departmentService = scopedProvider.GetRequiredService<IDepartmentService>();

            try
            {
                switch (action)
                {
                    case 0:
                        DepartmentCreateDto? createDto = JsonSerializer.Deserialize<DepartmentCreateDto>(jsonPayload);

                        if (createDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para crear departamento.");

                        Department newDepartment = new()
                        {
                            Name = createDto.Name,
                            LocationId = createDto.LocationId
                        };

                        Response<bool> insertResponse = await _departmentService.InsertAsync(newDepartment);
                        return JsonSerializer.Serialize(insertResponse);

                    case 1:
                        DepartmentUpdateDto? updateDto = JsonSerializer.Deserialize<DepartmentUpdateDto>(jsonPayload);

                        if (updateDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para actualizar departamento.");

                        Department departmentToUpdate = new()
                        {
                            Id = updateDto.Id,
                            Name = updateDto.Name,
                            LocationId = updateDto.LocationId
                        };

                        Response<bool> updateResponse = await _departmentService.UpdateAsync(departmentToUpdate);
                        return JsonSerializer.Serialize(updateResponse);

                    case 2:
                        Response<IEnumerable<Department>> responseList = await _departmentService.GetAllAsync();

                        if (!responseList.IsSuccess)
                            return JsonSerializer.Serialize(Response<List<DepartmentResponseDto>>.Fail(responseList.Message));

                        List<DepartmentResponseDto> list = [.. responseList.Data!.Select(p => new DepartmentResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            LocationId = p.LocationId
                        })];

                        return JsonSerializer.Serialize(Response<List<DepartmentResponseDto>>.Success(list, "Departamentos recuperados con éxito."));

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<Department> departmentResponse = await _departmentService.GetByIdAsync(searchedId);

                        if (!departmentResponse.IsSuccess)
                            return JsonSerializer.Serialize(Response<DepartmentResponseDto>.Fail(departmentResponse.Message));

                        DepartmentResponseDto responseDto = new()
                        {
                            Id = departmentResponse.Data!.Id,
                            Name = departmentResponse.Data.Name,
                            LocationId = departmentResponse.Data.LocationId
                        };

                        return JsonSerializer.Serialize(Response<DepartmentResponseDto>.Success(responseDto));

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<bool> deleteResponse = await _departmentService.DeleteAsync(deletedId);
                        return JsonSerializer.Serialize(deleteResponse);

                    default:
                        return SerializeHelper.SerializeFail("Acción no implementada.");
                }
            }
            catch (Exception ex)
            {
                return SerializeHelper.SerializeFail($"ERROR EN DEPARTMENT HANDLER: {ex.Message}");
            }
        }
    }
}
