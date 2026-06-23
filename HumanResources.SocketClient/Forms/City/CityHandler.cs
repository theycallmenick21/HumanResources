using HumanResources.Domain.Enums;
using HumanResources.SocketClient.Forms.Base;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.City
{
    public class CityHandler : BaseHandler
    {
        public string HandleForm(int action, int? idPredefinido = null)
        {
            return action switch
            {
                0 => CreateCityForm(),
                1 => UpdateCityForm(idPredefinido),
                3 => GetCityByIdForm(),
                5 => DeleteteCityForm(idPredefinido),
                _ => ""
            };
        }

        private static string CreateCityForm()
        {
            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: 1, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }

        private static string UpdateCityForm(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.City);

            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: id, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }

        private static string GetCityByIdForm()
        {
            int id = askId(EntitiesEnum.City);

            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: id, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }

        private static string DeleteteCityForm(int? idPredefinido) => JsonSerializer.Serialize(new { Id = idPredefinido ?? askId(EntitiesEnum.City) });
    }
}
