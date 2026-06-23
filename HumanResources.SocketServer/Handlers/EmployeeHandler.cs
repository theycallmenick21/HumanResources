using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Employee;
using HumanResources.Shared.Wrappers;
using HumanResources.SocketServer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class EmployeeHandler
    {
        internal static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            IEmployeeService _employeeService = scopedProvider.GetRequiredService<IEmployeeService>();

            try
            {
                switch (action)
                {
                    case 0:
                        EmployeeCreateDto? createDto = JsonSerializer.Deserialize<EmployeeCreateDto>(jsonPayload);

                        if (createDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para crear empleado.");

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

                        Response<bool> insertResponse = await _employeeService.InsertEmployeeAsync(newEmployee);
                        return JsonSerializer.Serialize(insertResponse);

                    case 1:
                        EmployeeUpdateDto? updateDto = JsonSerializer.Deserialize<EmployeeUpdateDto>(jsonPayload);

                        if (updateDto == null)
                            return SerializeHelper.SerializeFail("JSON inválido para actualizar empleado.");

                        Employee employeeToUpdate = new()
                        {
                            Id = updateDto.Id,
                            FirstName = updateDto.FirstName,
                            LastName = updateDto.LastName,
                            Email = updateDto.Email,
                            HiringDate = updateDto.HiringDate,
                            Salary = updateDto.Salary,
                            RoleId = updateDto.RoleId,
                            DepartmentId = updateDto.DepartmentId
                        };

                        Response<bool> updateResponse = await _employeeService.UpdateEmployeeAsync(employeeToUpdate);
                        return JsonSerializer.Serialize(updateResponse);

                    case 2:
                        Response<IEnumerable<Employee>> responseList = await _employeeService.GetAllAsync();

                        if (!responseList.IsSuccess)
                            return JsonSerializer.Serialize(Response<List<EmployeeResponseDto>>.Fail(responseList.Message));

                        List<EmployeeResponseDto> list = [.. responseList.Data!.Select(p => new EmployeeResponseDto
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

                        return JsonSerializer.Serialize(Response<List<EmployeeResponseDto>>.Success(list, "Empleados recuperados con éxito."));

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int searchedId = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<Employee> employeeResponse = await _employeeService.GetByIdAsync(searchedId);

                        if (!employeeResponse.IsSuccess)
                            return JsonSerializer.Serialize(Response<EmployeeResponseDto>.Fail(employeeResponse.Message));

                        EmployeeResponseDto responseDto = new()
                        {
                            Id = employeeResponse.Data!.Id,
                            FirstName = employeeResponse.Data.FirstName,
                            LastName = employeeResponse.Data.LastName,
                            Email = employeeResponse.Data.Email,
                            HiringDate = employeeResponse.Data.HiringDate,
                            Salary = employeeResponse.Data.Salary,
                            RoleId = employeeResponse.Data.RoleId,
                            DepartmentId = employeeResponse.Data.DepartmentId
                        };

                        return JsonSerializer.Serialize(Response<EmployeeResponseDto>.Success(responseDto));

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        Response<bool> deleteResponse = await _employeeService.DeleteEmployeeAsync(deletedId);
                        return JsonSerializer.Serialize(deleteResponse);

                    default:
                        return SerializeHelper.SerializeFail("Acción no implementada.");
                }
            }
            catch (Exception ex)
            {
                return SerializeHelper.SerializeFail($"ERROR EN EMPLOYEE HANDLER: {ex.Message}");
            }
        }
    }
}
