using HumanResources.Domain.Enums;
using System.Text.Json;

namespace HumanResources.SocketClient.Helpers
{
    public static class JsonExtractor
    {
        public static int ExtractRealId(EntitiesEnum entity, JsonElement item)
        {
            try
            {
                if (item.TryGetProperty("Id", out JsonElement idElement))
                    return idElement.GetInt32();

                return entity switch
                {
                    EntitiesEnum.City => item.GetProperty("Id").GetInt32(),
                    EntitiesEnum.Country => item.GetProperty("Id").GetInt32(),
                    EntitiesEnum.Department => item.GetProperty("Id").GetInt32(),
                    EntitiesEnum.Employee => item.GetProperty("Id").GetInt32(),
                    EntitiesEnum.Location => item.GetProperty("Id").GetInt32(),
                    EntitiesEnum.Record => item.GetProperty("Id").GetInt32(),
                    EntitiesEnum.Role => item.GetProperty("Id").GetInt32(),
                    _ => throw new Exception($"No se encontró la propiedad ID para {entity}")
                };
            }
            catch
            {
                return 0;
            }
        }

        public static string ExtractDisplayString(EntitiesEnum entity, JsonElement item)
        {
            try
            {
                return entity switch
                {
                    EntitiesEnum.City => $"ID: {item.GetProperty("Id")} | Ciudad: {item.GetProperty("Name")}",
                    EntitiesEnum.Country => $"ID: {item.GetProperty("Id")} | País: {item.GetProperty("Name")}",
                    EntitiesEnum.Department => $"ID: {item.GetProperty("Id")} | Depto: {item.GetProperty("Name")}",
                    EntitiesEnum.Employee => $"ID: {item.GetProperty("Id")} | Empleado: {item.GetProperty("Email")}",
                    EntitiesEnum.Location => $"ID: {item.GetProperty("Id")} | Ubicación: {item.GetProperty("Address")}",
                    EntitiesEnum.Record => $"ID: {item.GetProperty("EmployeeId")} | Registro: {item.GetProperty("EmployeeId")}",
                    EntitiesEnum.Role => $"ID: {item.GetProperty("Id")} | Rol: {item.GetProperty("Name")}",
                    _ => item.ToString()
                };
            }
            catch
            {
                return "Error leyendo registro";
            }
        }
    }
}