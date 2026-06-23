using HumanResources.Domain.Enums;
using HumanResources.Shared.DTOs.Country;
using HumanResources.SocketClient.Forms.Base;
using HumanResources.SocketClient.Helpers;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Country
{
    public class CountryHandler : BaseHandler
    {
        public string HandleForm(int action, int? idPredefinido = null, JsonElement? dbData = null) => action switch
        {
            0 => CreateCountryForm(),
            1 => UpdateCountryForm(dbData!.Value),
            3 => GetCountryByIdForm(),
            5 => DeleteteCountryForm(idPredefinido),
            _ => ""
        };

        private static string CreateCountryForm()
        {
            Console.Write("Ingrese el Nombre del País: ");
            string name = Console.ReadLine()!;

            CountryCreateDto CountryCreateDto = new() { Name = name };
            return JsonSerializer.Serialize(CountryCreateDto);
        }

        public static string UpdateCountryForm(JsonElement dbData)
        {
            int id = dbData.GetProperty("Id").GetInt32();
            string storedName = dbData.GetProperty("Name").GetString()!;

            Console.WriteLine("\n[~] MODO EDICIÓN");
            Console.WriteLine("Instrucción: Escriba el nuevo valor o presione ENTER para mantener el valor actual.\n");

            string newName = ConsoleHelper.ShowValuesToUpdate("Nombre del País", storedName);

            CountryUpdateDto data = new()
            {
                Id = id,
                Name = newName
            };

            return JsonSerializer.Serialize(data);
        }

        private static string GetCountryByIdForm() => JsonSerializer.Serialize(new { Id = AskId(EntitiesEnum.Country) });

        private static string DeleteteCountryForm(int? predefinedId) => JsonSerializer.Serialize(new { Id = predefinedId ?? AskId(EntitiesEnum.Country) });
    }
}
