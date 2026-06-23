using HumanResources.Domain.Enums;
using HumanResources.Shared.DTOs.Department;
using HumanResources.SocketClient.Forms.Base;
using HumanResources.SocketClient.Helpers;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Department
{
    public class DepartmentHandler : BaseHandler
    {
        public string HandleForm(int action, int? idPredefinido = null, JsonElement? dbData = null) => action switch
        {
            0 => CreateDepartmentForm(),
            1 => UpdateDepartmentForm(dbData!.Value),
            3 => GetDepartmentByIdForm(),
            5 => DeleteteDepartmentForm(idPredefinido),
            _ => ""
        };

        private static string CreateDepartmentForm()
        {
            Console.Write("Ingrese el Nombre del Departamento: ");
            string name = Console.ReadLine()!;


            Console.Write("Ingrese el ID de la Localización asociada: ");
            int locationId;
            while (true)
            {
                string input = Console.ReadLine()!;
                if (int.TryParse(input, out locationId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            DepartmentCreateDto DepartmentCreateDto = new() { Name = name, LocationId = locationId };
            return JsonSerializer.Serialize(DepartmentCreateDto);
        }

        public static string UpdateDepartmentForm(JsonElement dbData)
        {
            int id = dbData.GetProperty("Id").GetInt32();
            string storedName = dbData.GetProperty("Name").GetString()!;
            int storedLocationId = dbData.GetProperty("LocationId").GetInt32();

            Console.WriteLine("\n[~] MODO EDICIÓN");
            Console.WriteLine("Instrucción: Escriba el nuevo valor o presione ENTER para mantener el valor actual.\n");

            string newName = ConsoleHelper.ShowValuesToUpdate("Nombre del Departamento", storedName);
            string newLocationStr = ConsoleHelper.ShowValuesToUpdate("ID de la Localización asociada", storedLocationId.ToString());

            int newLocationId;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("ID de la Localización", storedLocationId.ToString());
                if (int.TryParse(input, out newLocationId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            DepartmentUpdateDto data = new()
            {
                Id = id,
                Name = newName,
                LocationId = newLocationId
            };

            return JsonSerializer.Serialize(data);
        }

        private static string GetDepartmentByIdForm() => JsonSerializer.Serialize(new { Id = AskId(EntitiesEnum.Department) });

        private static string DeleteteDepartmentForm(int? predefinedId) => JsonSerializer.Serialize(new { Id = predefinedId ?? AskId(EntitiesEnum.Department) });
    }
}
