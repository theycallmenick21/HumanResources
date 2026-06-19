using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Record;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class RecordHandler
    {
        public static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            IRecordService _recordService = scopedProvider.GetRequiredService<IRecordService>();

            try
            {
                switch (action)
                {
                    case 0:
                        RecordCreateDto? createDto = JsonSerializer.Deserialize<RecordCreateDto>(jsonPayload);
                        if (createDto == null) return "ERROR: JSON inválido para crear registro.";

                        Record newRecord = new()
                        {
                            EmployeeId = createDto.EmployeeId,
                            StartDate = createDto.StartDate,
                            EndDate = createDto.EndDate,
                            RoleId = createDto.RoleId,
                            DepartmentId = createDto.DepartmentId
                        };
                        bool inserted = await _recordService.InsertAsync(newRecord);

                        return inserted ? "Registro creado exitosamente." : "ERROR: No se pudo crear el registro.";

                    case 2:
                        List<Record> list = await _recordService.GetAllAsync();

                        List<RecordResponseDto> listaResponse = [.. list.Select(p => new RecordResponseDto
                        {
                            EmployeeId = p.EmployeeId,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            RoleId = p.RoleId,
                            DepartmentId = p.DepartmentId
                        })];

                        return JsonSerializer.Serialize(listaResponse);

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int idBuscar = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Record? record = await _recordService.GetByIdAsync(idBuscar);
                        if (record == null) return "ERROR: Registro no encontrado.";

                        RecordResponseDto responseDto = new()
                        {
                            EmployeeId = record.EmployeeId,
                            StartDate = record.StartDate,
                            EndDate = record.EndDate,
                            RoleId = record.RoleId,
                            DepartmentId = record.DepartmentId
                        };

                        return JsonSerializer.Serialize(new List<RecordResponseDto> { responseDto });

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        bool deleted = await _recordService.DeleteAsync(deletedId);
                        return deleted ? "Registro borrado exitosamente." : "ERROR: No se pudo borrar o el ID no existe.";

                    default:
                        return "ERROR: Acción no implementada.";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR EN RECORD HANDLER: {ex.Message}";
            }
        }
    }
}
