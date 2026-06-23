using HumanResources.Domain.Enums;
using HumanResources.Shared.DTOs.City;
using HumanResources.SocketClient.Forms.Base;
using HumanResources.SocketClient.Helpers;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.City
{
    public class CityHandler : BaseHandler
    {
        public string HandleForm(int action, int? idPredefinido = null, JsonElement? dbData = null) => action switch
        {
            0 => CreateCityForm(),
            1 => UpdateCityForm(dbData!.Value),
            3 => GetCityByIdForm(),
            5 => DeleteteCityForm(idPredefinido),
            _ => ""
        };

        private static string CreateCityForm()
        {
            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string name = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int countryId = int.Parse(Console.ReadLine()!);

            CityCreateDto cityCreateDto = new() { Name = name, CountryId = countryId };
            return JsonSerializer.Serialize(cityCreateDto);
        }

        public static string UpdateCityForm(JsonElement dbData)
        {
            int id = dbData.GetProperty("Id").GetInt32();
            string nombreActual = dbData.GetProperty("Name").GetString()!;
            int paisActual = dbData.GetProperty("CountryId").GetInt32();

            Console.WriteLine("\n[~] MODO EDICIÓN");
            Console.WriteLine("Instrucción: Escriba el nuevo valor o presione ENTER para mantener el valor actual.\n");

            string nuevoNombre = ConsoleHelper.ShowValuesToUpdate("Nombre de la Ciudad", nombreActual);

            string nuevoPaisStr = ConsoleHelper.ShowValuesToUpdate("ID del País asociado", paisActual.ToString());
            int nuevoPaisId = int.Parse(nuevoPaisStr);

            CityUpdateDto data = new()
            {
                Id = id,
                Name = nuevoNombre,
                CountryId = nuevoPaisId
            };

            return JsonSerializer.Serialize(data);
        }

        private static string GetCityByIdForm() => JsonSerializer.Serialize(new { Id = AskId(EntitiesEnum.City) });

        private static string DeleteteCityForm(int? idPredefinido) => JsonSerializer.Serialize(new { Id = idPredefinido ?? AskId(EntitiesEnum.City) });
    }
}
