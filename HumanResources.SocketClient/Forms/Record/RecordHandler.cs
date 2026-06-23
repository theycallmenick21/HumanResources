using HumanResources.Domain.Enums;
using HumanResources.Shared.DTOs.Record;
using HumanResources.SocketClient.Forms.Base;
using HumanResources.SocketClient.Helpers;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Record
{
    public class RecordHandler : BaseHandler
    {
        public string HandleForm(int action, int? idPredefinido = null, JsonElement? dbData = null) => action switch
        {
            1 => UpdateRecordForm(dbData!.Value),
            3 => GetRecordByIdForm(),
            _ => ""
        };

        public static string UpdateRecordForm(JsonElement dbData)
        {
            int storedEmployeeId = dbData.GetProperty("EmployeeId").GetInt32();
            int storedDepartmentId = dbData.GetProperty("DepartmentId").GetInt32();
            int storedRoleId = dbData.GetProperty("RoleId").GetInt32();
            string storedStartDate = dbData.GetProperty("StartDate").GetDateTime().ToString("yyyy-MM-dd");
            string storedEndDate = dbData.GetProperty("EndDate").GetDateTime().ToString("yyyy-MM-dd");

            Console.WriteLine("\n[~] MODO EDICIÓN");
            Console.WriteLine("Instrucción: Escriba el nuevo valor o presione ENTER para mantener el valor actual.\n");

            int newEmployeeId;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("ID del Empleado", storedEmployeeId.ToString());
                if (int.TryParse(input, out newEmployeeId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            int newDepartmentId;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("ID del Departamento", storedDepartmentId.ToString());
                if (int.TryParse(input, out newDepartmentId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            int newRoleId;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("ID del Rol", storedRoleId.ToString());
                if (int.TryParse(input, out newRoleId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            DateTime newStartDate;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("Fecha de Inicio (YYYY-MM-DD)", storedStartDate);
                if (DateTime.TryParse(input, out newStartDate)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Formato de fecha inválido. Use el formato YYYY-MM-DD.");
                Console.ResetColor();
            }

            DateTime newEndDate;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("Fecha de Fin (YYYY-MM-DD)", storedEndDate);

                if (!DateTime.TryParse(input, out newEndDate))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Formato de fecha inválido. Use el formato YYYY-MM-DD.");
                    Console.ResetColor();
                }
                else if (newEndDate < newStartDate)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: La fecha de fin no puede ser anterior a la fecha de inicio.");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            RecordUpdateDto data = new()
            {
                EmployeeId = newEmployeeId,
                DepartmentId = newDepartmentId,
                RoleId = newRoleId,
                StartDate = newStartDate,
                EndDate = newEndDate
            };

            return JsonSerializer.Serialize(data);
        }

        private static string GetRecordByIdForm() => JsonSerializer.Serialize(new { Id = AskId(EntitiesEnum.Record) });
    }
}
