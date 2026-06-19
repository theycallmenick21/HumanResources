using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Shared.DTOs.Role;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HumanResources.SocketServer.Handlers
{
    internal class RoleHandler
    {
        public static async Task<string> ProcessAsync(int action, string jsonPayload, IServiceProvider scopedProvider)
        {
            IRoleService _roleService = scopedProvider.GetRequiredService<IRoleService>();

            try
            {
                switch (action)
                {
                    case 0:
                        RoleCreateDto? createDto = JsonSerializer.Deserialize<RoleCreateDto>(jsonPayload);
                        if (createDto == null) return "ERROR: JSON inválido para crear rol.";

                        Role newRole = new()
                        {
                            Name = createDto.Name,
                            MinSalary = createDto.MinSalary,
                            MaxSalary = createDto.MaxSalary
                        };
                        bool inserted = await _roleService.InsertAsync(newRole);

                        return inserted ? "Rol creado exitosamente." : "ERROR: No se pudo crear el rol.";

                    case 2:
                        List<Role> list = await _roleService.GetAllAsync();

                        List<RoleResponseDto> listaResponse = [.. list.Select(p => new RoleResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            MinSalary = p.MinSalary,
                            MaxSalary = p.MaxSalary
                        })];

                        return JsonSerializer.Serialize(listaResponse);

                    case 3:
                        JsonDocument searchDoc = JsonDocument.Parse(jsonPayload);
                        int idBuscar = searchDoc.RootElement.GetProperty("Id").GetInt32();

                        Role? role = await _roleService.GetByIdAsync(idBuscar);
                        if (role == null) return "ERROR: Rol no encontrado.";

                        RoleResponseDto responseDto = new()
                        {
                            Id = role.Id,
                            Name = role.Name,
                            MinSalary = role.MinSalary,
                            MaxSalary = role.MaxSalary
                        };

                        return JsonSerializer.Serialize(new List<RoleResponseDto> { responseDto });

                    case 5:
                        JsonDocument deleteDoc = JsonDocument.Parse(jsonPayload);
                        int deletedId = deleteDoc.RootElement.GetProperty("Id").GetInt32();

                        bool deleted = await _roleService.DeleteAsync(deletedId);
                        return deleted ? "Registro borrado exitosamente." : "ERROR: No se pudo borrar o el ID no existe.";

                    default:
                        return "ERROR: Acción no implementada.";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR EN COUNTRY HANDLER: {ex.Message}";
            }
        }
    }
}
