using HumanResources.Domain.Enums;
using HumanResources.SocketServer.Handlers;
using HumanResources.SocketServer.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace HumanResources.SocketServer.Routing
{
    public static class RequestRouter
    {
        public static async Task<string> RouteRequestAsync(string rawMessage, IServiceProvider serviceProvider)
        {
            try
            {
                string[] parts = rawMessage.Split('|', 4);
                if (parts.Length < 4) return SerializeHelper.SerializeFail("ERROR: Formato de mensaje incorrecto.");

                string user = parts[0];
                if (!Enum.TryParse(parts[1], out EntitiesEnum entity)) return SerializeHelper.SerializeFail("ERROR: Entidad no reconocida.");
                if (!int.TryParse(parts[2], out int action)) return SerializeHelper.SerializeFail("ERROR: Acción no válida.");
                string payloadJson = parts[3];

                using IServiceScope scope = serviceProvider.CreateScope();

                return entity switch
                {
                    EntitiesEnum.City => await CityHandler.ProcessAsync(action, payloadJson, scope.ServiceProvider),
                    EntitiesEnum.Country => await CountryHandler.ProcessAsync(action, payloadJson, scope.ServiceProvider),
                    EntitiesEnum.Department => await DepartmentHandler.ProcessAsync(action, payloadJson, scope.ServiceProvider),
                    EntitiesEnum.Employee => await EmployeeHandler.ProcessAsync(action, payloadJson, scope.ServiceProvider),
                    EntitiesEnum.Location => await LocationHandler.ProcessAsync(action, payloadJson, scope.ServiceProvider),
                    EntitiesEnum.Record => await RecordHandler.ProcessAsync(action, payloadJson, scope.ServiceProvider),
                    EntitiesEnum.Role => await RoleHandler.ProcessAsync(action, payloadJson, scope.ServiceProvider),
                    _ => SerializeHelper.SerializeFail($"ERROR: La entidad {entity} no está soportada.")
                };
            }
            catch (Exception ex)
            {
                return SerializeHelper.SerializeFail($"ERROR DEL SERVIDOR: {ex.Message}");
            }
        }
    }
}