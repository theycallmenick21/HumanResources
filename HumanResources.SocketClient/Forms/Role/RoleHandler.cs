using HumanResources.Domain.Enums;
using HumanResources.Shared.DTOs.Role;
using HumanResources.SocketClient.Forms.Base;
using HumanResources.SocketClient.Helpers;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Role
{
    public class RoleHandler : BaseHandler
    {
        public string HandleForm(int action, int? idPredefinido = null, JsonElement? dbData = null) => action switch
        {
            0 => CreateRoleForm(),
            1 => UpdateRoleForm(dbData!.Value),
            3 => GetRoleByIdForm(),
            5 => DeleteteRoleForm(idPredefinido),
            _ => ""
        };

        private static string CreateRoleForm()
        {
            Console.Write("Ingrese el Nombre del Rol: ");
            string name = Console.ReadLine()!;

            decimal minSalary;
            Console.Write("Ingrese el Salario Mínimo: ");
            while (!decimal.TryParse(Console.ReadLine(), out minSalary))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Número inválido. Ingrese el Salario Mínimo nuevamente: ");
                Console.ResetColor();
            }

            decimal maxSalary = 0;
            bool isValidMaxSalary = false;

            while (!isValidMaxSalary)
            {
                Console.Write("Ingrese el Salario Máximo: ");

                if (!decimal.TryParse(Console.ReadLine(), out maxSalary))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Por favor ingrese un número válido.");
                    Console.ResetColor();
                }
                else if (maxSalary <= minSalary)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: El salario máximo debe ser estrictamente mayor que el mínimo ({minSalary:C}).");
                    Console.ResetColor();
                }
                else
                {
                    isValidMaxSalary = true;
                }
            }

            RoleCreateDto roleCreateDto = new() { Name = name, MinSalary = minSalary, MaxSalary = maxSalary };
            return JsonSerializer.Serialize(roleCreateDto);
        }

        public static string UpdateRoleForm(JsonElement dbData)
        {
            int id = dbData.GetProperty("Id").GetInt32();
            string storedName = dbData.GetProperty("Name").GetString()!;
            decimal storedMinSalary = dbData.GetProperty("MinSalary").GetDecimal();
            decimal storedMaxSalary = dbData.GetProperty("MaxSalary").GetDecimal();

            Console.WriteLine("\n[~] MODO EDICIÓN");
            Console.WriteLine("Instrucción: Escriba el nuevo valor o presione ENTER para mantener el valor actual.\n");

            string newName = ConsoleHelper.ShowValuesToUpdate("Nombre del Rol", storedName);

            decimal newMinSalary;
            while (true)
            {
                string inputMin = ConsoleHelper.ShowValuesToUpdate("Salario Mínimo", storedMinSalary.ToString());

                if (decimal.TryParse(inputMin, out newMinSalary))
                    break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número válido.");
                Console.ResetColor();
            }

            decimal newMaxSalary;
            while (true)
            {
                string inputMax = ConsoleHelper.ShowValuesToUpdate("Salario Máximo", storedMaxSalary.ToString());

                if (!decimal.TryParse(inputMax, out newMaxSalary))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Por favor ingrese un número válido.");
                    Console.ResetColor();
                }
                else if (newMaxSalary <= newMinSalary)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: El salario máximo debe ser estrictamente mayor que el mínimo ({newMinSalary:C}).");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            RoleUpdateDto data = new()
            {
                Id = id,
                Name = newName,
                MinSalary = newMinSalary,
                MaxSalary = newMaxSalary
            };

            return JsonSerializer.Serialize(data);
        }

        private static string GetRoleByIdForm() => JsonSerializer.Serialize(new { Id = AskId(EntitiesEnum.Role) });

        private static string DeleteteRoleForm(int? predefinedId) => JsonSerializer.Serialize(new { Id = predefinedId ?? AskId(EntitiesEnum.Role) });
    }
}
