using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Department;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class DepartmentHandler
    {
        public static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            IDepartmentService _departmentService = scopedProvider.GetRequiredService<IDepartmentService>();

            try
            {
                switch (action)
                {
                    case 0:
                        DepartmentCreateDto? createDto = JsonSerializer.Deserialize<DepartmentCreateDto>(jsonPayload);
                        if (createDto == null) return "ERROR: JSON inválido para crear departamento.";

                        Department newDepartment = new() { Name = createDto.Name };
                        bool inserted = await _departmentService.InsertAsync(newDepartment);

                        return inserted ? "Departamento creado exitosamente." : "ERROR: No se pudo crear el departamento.";

                    case 2:
                        List<Department> list = await _departmentService.GetAllAsync();

                        List<DepartmentResponseDto> responseList = [.. list.Select(p => new DepartmentResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            LocationId = p.LocationId
                        })];

                        return JsonSerializer.Serialize(responseList);

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Department? department = await _departmentService.GetByIdAsync(searchedId);
                        if (department == null) return "ERROR: Departamento no encontrado.";

                        DepartmentResponseDto responseDto = new() { Id = department.Id, Name = department.Name };
                        return JsonSerializer.Serialize(new List<DepartmentResponseDto> { responseDto });

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        bool deleted = await _departmentService.DeleteAsync(deletedId);
                        return deleted ? "Registro borrado exitosamente." : "ERROR: No se pudo borrar o el ID no existe.";

                    default:
                        return "ERROR: Acción no implementada.";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR EN DEPARTMENT HANDLER: {ex.Message}";
            }
        }
    }
}
