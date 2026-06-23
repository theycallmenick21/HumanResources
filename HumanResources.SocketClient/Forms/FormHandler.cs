using HumanResources.Domain.Enums;
using HumanResources.SocketClient.Forms.City;
using HumanResources.SocketClient.Forms.Country;
using HumanResources.SocketClient.Forms.Department;
using HumanResources.SocketClient.Forms.Employee;
using HumanResources.SocketClient.Forms.Location;
using HumanResources.SocketClient.Forms.Record;
using HumanResources.SocketClient.Forms.Role;

namespace HumanResources.SocketClient.Forms
{
    public static class FormHandler
    {
        public static string ProccessForm(EntitiesEnum entity, int action, int? predefinedId = null)
        {
            return entity switch
            {
                EntitiesEnum.City => new CityHandler().HandleForm(action, predefinedId),
                EntitiesEnum.Country => new CountryHandler().HandleForm(action, predefinedId),
                EntitiesEnum.Department => new DepartmentHandler().HandleForm(action, predefinedId),
                EntitiesEnum.Employee => new EmployeeHandler().HandleForm(action, predefinedId),
                EntitiesEnum.Location => new LocationHandler().HandleForm(action, predefinedId),
                EntitiesEnum.Record => new RecordHandler().HandleForm(action, predefinedId),
                EntitiesEnum.Role => new RoleHandler().HandleForm(action, predefinedId),
                _ => throw new NotImplementedException($"El formulario para {entity} no está implementado.")
            };
        }
    }
}