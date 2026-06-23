using HumanResources.Domain.Enums;
using HumanResources.Shared.DTOs.Location;
using HumanResources.SocketClient.Forms.Base;
using HumanResources.SocketClient.Helpers;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Location
{
    public class LocationHandler : BaseHandler
    {
        public string HandleForm(int action, int? idPredefinido = null, JsonElement? dbData = null) => action switch
        {
            0 => CreateLocationForm(),
            1 => UpdateLocationForm(dbData!.Value),
            3 => GetLocationByIdForm(),
            5 => DeleteteLocationForm(idPredefinido),
            _ => ""
        };

        private static string CreateLocationForm()
        {
            Console.Write("Ingrese el Nombre de la locación: ");
            string address = Console.ReadLine()!;

            Console.Write("Ingrese el ID de la Ciudad asociada: ");
            int cityId;
            while (true)
            {
                string input = Console.ReadLine()!;
                if (int.TryParse(input, out cityId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            LocationCreateDto locationCreateDto = new() { Address = address, CityId = cityId };
            return JsonSerializer.Serialize(locationCreateDto);
        }

        public static string UpdateLocationForm(JsonElement dbData)
        {
            int id = dbData.GetProperty("Id").GetInt32();
            string storedAddress = dbData.GetProperty("Address").GetString()!;
            int storedCityId = dbData.GetProperty("CityId").GetInt32();

            Console.WriteLine("\n[~] MODO EDICIÓN");
            Console.WriteLine("Instrucción: Escriba el nuevo valor o presione ENTER para mantener el valor actual.\n");

            string newAddress = ConsoleHelper.ShowValuesToUpdate("Dirección", storedAddress);

            int newCityId;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("ID de la Ciudad", storedCityId.ToString());
                if (int.TryParse(input, out newCityId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            LocationUpdateDto data = new()
            {
                Id = id,
                Address = newAddress,
                CityId = newCityId
            };

            return JsonSerializer.Serialize(data);
        }

        private static string GetLocationByIdForm() => JsonSerializer.Serialize(new { Id = AskId(EntitiesEnum.Location) });

        private static string DeleteteLocationForm(int? predefinedId) => JsonSerializer.Serialize(new { Id = predefinedId ?? AskId(EntitiesEnum.Location) });
    }
}
