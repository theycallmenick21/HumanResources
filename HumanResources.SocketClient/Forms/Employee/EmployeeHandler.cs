using HumanResources.Domain.Enums;
using HumanResources.SocketClient.Forms.Base;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Employee
{
    public class EmployeeHandler : BaseHandler
    {
        public string HandleForm(int accion, int? idPredefinido = null)
        {
            return "";
        }

        private static string GetEmployee(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.City);

            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: id, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }
    }
}
