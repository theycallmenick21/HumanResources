using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Employee;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class EmployeeHandler
    {
        public static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            IEmployeeService _employeeService = scopedProvider.GetRequiredService<IEmployeeService>();

            try
            {
                switch (action)
                {
                    case 0:
                        EmployeeCreateDto? createDto = JsonSerializer.Deserialize<EmployeeCreateDto>(jsonPayload);
                        if (createDto == null) return "ERROR: JSON inválido para crear empleado.";

                        Employee newEmployee = new()
                        {
                            FirstName = createDto.FirstName,
                            LastName = createDto.LastName,
                            Email = createDto.Email,
                            HiringDate = createDto.HiringDate,
                            Salary = createDto.Salary,
                            RoleId = createDto.RoleId,
                            DepartmentId = createDto.DepartmentId
                        };

                        bool inserted = await _employeeService.InsertAsync(newEmployee);

                        return inserted ? "Empleado creado exitosamente." : "ERROR: No se pudo crear el empleado.";

                    case 2:
                        List<Employee> list = await _employeeService.GetAllAsync();

                        List<EmployeeResponseDto> responseList = [.. list.Select(p => new EmployeeResponseDto
                        {
                            Id = p.Id,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            Email = p.Email,
                            HiringDate = p.HiringDate,
                            Salary = p.Salary,
                            RoleId = p.RoleId,
                            DepartmentId = p.DepartmentId
                        })];

                        return JsonSerializer.Serialize(responseList);

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int idBuscar = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Employee? employee = await _employeeService.GetByIdAsync(idBuscar);
                        if (employee == null) return "ERROR: Empleado no encontrado.";

                        EmployeeResponseDto responseDto = new()
                        {
                            Id = employee.Id,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            HiringDate = employee.HiringDate,
                            Salary = employee.Salary,
                            RoleId = employee.RoleId,
                            DepartmentId = employee.DepartmentId
                        };

                        return JsonSerializer.Serialize(new List<EmployeeResponseDto> { responseDto });

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        bool deleted = await _employeeService.DeleteAsync(deletedId);
                        return deleted ? "Registro borrado exitosamente." : "ERROR: No se pudo borrar o el ID no existe.";

                    default:
                        return "ERROR: Acción no implementada.";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR EN EMPLOYEE HANDLER: {ex.Message}";
            }
        }
    }
}
