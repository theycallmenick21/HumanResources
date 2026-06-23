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
            int countryId;
            while (true)
            {
                string input = Console.ReadLine()!;
                if (int.TryParse(input, out countryId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            CityCreateDto cityCreateDto = new() { Name = name, CountryId = countryId };
            return JsonSerializer.Serialize(cityCreateDto);
        }

        public static string UpdateCityForm(JsonElement dbData)
        {
            int id = dbData.GetProperty("Id").GetInt32();
            string storedName = dbData.GetProperty("Name").GetString()!;
            int storedCoundtryId = dbData.GetProperty("CountryId").GetInt32();

            Console.WriteLine("\n[~] MODO EDICIÓN");
            Console.WriteLine("Instrucción: Escriba el nuevo valor o presione ENTER para mantener el valor actual.\n");

            string newName = ConsoleHelper.ShowValuesToUpdate("Nombre de la Ciudad", storedName);
            string newCountryStr = ConsoleHelper.ShowValuesToUpdate("ID del País asociado", storedCoundtryId.ToString());

            int newCountryId;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("ID del País", storedCoundtryId.ToString());
                if (int.TryParse(input, out newCountryId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            CityUpdateDto data = new()
            {
                Id = id,
                Name = newName,
                CountryId = newCountryId
            };

            return JsonSerializer.Serialize(data);
        }

        private static string GetCityByIdForm() => JsonSerializer.Serialize(new { Id = AskId(EntitiesEnum.City) });

        private static string DeleteteCityForm(int? predefinedId) => JsonSerializer.Serialize(new { Id = predefinedId ?? AskId(EntitiesEnum.City) });
    }
}
