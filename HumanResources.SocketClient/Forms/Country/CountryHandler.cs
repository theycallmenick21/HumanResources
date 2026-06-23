using HumanResources.SocketClient.Forms.Base;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Country
{
    public class CountryHandler : BaseHandler
    {
        public string HandleForm(int accion, int? idPredefinido = null)
        {
            return "";
        }

        private static string GetCountry(int? idPredefinido)
        {
            int id = 0;

            Console.Write("Ingrese el Nombre del País: ");
            string nombre = Console.ReadLine()!;

            (int Id, string Name) data = (Id: id, Name: nombre);
            return JsonSerializer.Serialize(data);
        }
    }
}
