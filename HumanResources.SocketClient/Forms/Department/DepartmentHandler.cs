using HumanResources.Domain.Enums;
using HumanResources.SocketClient.Forms.Base;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Department
{
    public class DepartmentHandler : BaseHandler
    {
        public string HandleForm(int accion, int? idPredefinido = null)
        {
            return "";
        }

        private static string GetDepartment(int? idPredefinido)
        {
            int id = idPredefinido ?? AskId(EntitiesEnum.Department);

            Console.Write("Ingrese el Nombre del Departamento: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID de la Locación asociada: ");
            int idLocacion = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int LocationId) data = (Id: id, Name: nombre, LocationId: idLocacion);
            return JsonSerializer.Serialize(data);
        }
    }
}
