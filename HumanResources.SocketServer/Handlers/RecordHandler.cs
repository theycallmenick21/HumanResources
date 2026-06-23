using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Record;
using HumanResources.Shared.Wrappers;
using HumanResources.SocketServer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class RecordHandler
    {
        internal static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            IRecordService _recordService = scopedProvider.GetRequiredService<IRecordService>();

            try
            {
                switch (action)
                {
                    case 0:
                        RecordCreateDto? createDto = JsonSerializer.Deserialize<RecordCreateDto>(jsonPayload);

                        if (createDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para crear registro.");

                        Record newRecord = new()
                        {
                            EmployeeId = createDto.EmployeeId,
                            StartDate = createDto.StartDate,
                            EndDate = createDto.EndDate,
                            RoleId = createDto.RoleId,
                            DepartmentId = createDto.DepartmentId
                        };

                        Response<bool> insertResponse = await _recordService.InsertAsync(newRecord);
                        return JsonSerializer.Serialize(insertResponse);

                    case 1:
                        RecordUpdateDto? updateDto = JsonSerializer.Deserialize<RecordUpdateDto>(jsonPayload);

                        if (updateDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para actualizar registro.");

                        Record recordToUpdate = new()
                        {
                            EmployeeId = updateDto.EmployeeId,
                            StartDate = updateDto.StartDate,
                            EndDate = updateDto.EndDate,
                            RoleId = updateDto.RoleId,
                            DepartmentId = updateDto.DepartmentId
                        };

                        Response<bool> updateResponse = await _recordService.UpdateAsync(recordToUpdate);
                        return JsonSerializer.Serialize(updateResponse);

                    case 2:
                        Response<IEnumerable<Record>> responseList = await _recordService.GetAllAsync();

                        if (!responseList.IsSuccess)
                            return JsonSerializer.Serialize(Response<List<RecordResponseDto>>.Fail(responseList.Message));

                        List<RecordResponseDto> list = [.. responseList.Data!.Select(p => new RecordResponseDto
                        {
                            EmployeeId = p.EmployeeId,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            RoleId = p.RoleId,
                            DepartmentId = p.DepartmentId
                        })];

                        return JsonSerializer.Serialize(Response<List<RecordResponseDto>>.Success(list, "Registros recuperados con éxito."));

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<Record> recordResponse = await _recordService.GetByIdAsync(searchedId);

                        if (!recordResponse.IsSuccess)
                            return JsonSerializer.Serialize(Response<RecordResponseDto>.Fail(recordResponse.Message));

                        RecordResponseDto responseDto = new()
                        {
                            EmployeeId = recordResponse.Data!.EmployeeId,
                            StartDate = recordResponse.Data.StartDate,
                            EndDate = recordResponse.Data.EndDate,
                            RoleId = recordResponse.Data.RoleId,
                            DepartmentId = recordResponse.Data.DepartmentId
                        };

                        return JsonSerializer.Serialize(Response<RecordResponseDto>.Success(responseDto));

                    default:
                        return SerializeHelper.SerializeFail("Acción no implementada.");
                }
            }
            catch (Exception ex)
            {
                return SerializeHelper.SerializeFail($"ERROR EN RECORD HANDLER: {ex.Message}");
            }
        }
    }
}
