using HumanResources.Domain.Enums;
using HumanResources.Shared.DTOs.Employee;
using HumanResources.SocketClient.Forms.Base;
using HumanResources.SocketClient.Helpers;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms.Employee
{
    public class EmployeeHandler : BaseHandler
    {
        public string HandleForm(int action, int? idPredefinido = null, JsonElement? dbData = null) => action switch
        {
            0 => CreateEmployeeForm(),
            1 => dbData.HasValue ? UpdateEmployeeForm(dbData.Value) : throw new ArgumentNullException(nameof(dbData), "Datos actuales requeridos para edición."),
            3 => GetEmployeeByIdForm(),
            5 => DeleteEmployeeForm(idPredefinido),
            _ => ""
        };

        private static string CreateEmployeeForm()
        {
            Console.Write("Ingrese el nombre del empleado: ");
            string firstName = Console.ReadLine()!;

            Console.Write("Ingrese el apellido del empleado: ");
            string lastName = Console.ReadLine()!;

            Console.Write("Ingrese el email del empleado: ");
            string email = Console.ReadLine()!;

            DateTime hiringDate;
            while (true)
            {
                Console.Write("Ingrese la fecha de contratación (YYYY-MM-DD): ");
                if (DateTime.TryParse(Console.ReadLine(), out hiringDate)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Formato de fecha inválido.");
                Console.ResetColor();
            }

            decimal salary;
            while (true)
            {
                Console.Write("Ingrese el salario del empleado: ");
                if (decimal.TryParse(Console.ReadLine(), out salary)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número válido.");
                Console.ResetColor();
            }

            Console.Write("Ingrese el ID del Rol asociado: ");
            int roleId;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out roleId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Por favor ingrese un número entero válido. Intente de nuevo: ");
                Console.ResetColor();
            }

            Console.Write("Ingrese el ID del Departamento asociado: ");
            int departmentId;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out departmentId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Por favor ingrese un número entero válido. Intente de nuevo: ");
                Console.ResetColor();
            }

            EmployeeCreateDto employeeCreateDto = new()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                HiringDate = hiringDate,
                Salary = salary,
                RoleId = roleId,
                DepartmentId = departmentId
            };

            return JsonSerializer.Serialize(employeeCreateDto);
        }

        public static string UpdateEmployeeForm(JsonElement dbData)
        {
            int id = dbData.GetProperty("Id").GetInt32();
            string storedFirstName = dbData.GetProperty("FirstName").GetString()!;
            string storedLastName = dbData.GetProperty("LastName").GetString()!;
            string storedEmail = dbData.GetProperty("Email").GetString()!;
            string storedHiringDate = dbData.GetProperty("HiringDate").GetDateTime().ToString("yyyy-MM-dd");
            decimal storedSalary = dbData.GetProperty("Salary").GetDecimal();
            int storedRoleId = dbData.GetProperty("RoleId").GetInt32();
            int storedDepartmentId = dbData.GetProperty("DepartmentId").GetInt32();

            Console.WriteLine("\n[~] MODO EDICIÓN");
            Console.WriteLine("Instrucción: Escriba el nuevo valor o presione ENTER para mantener el valor actual.\n");

            string newFirstName = ConsoleHelper.ShowValuesToUpdate("Nombre del empleado", storedFirstName);
            string newLastName = ConsoleHelper.ShowValuesToUpdate("Apellido del empleado", storedLastName);
            string newEmail = ConsoleHelper.ShowValuesToUpdate("Email", storedEmail);

            DateTime newHiringDate;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("Fecha de Contratación (YYYY-MM-DD)", storedHiringDate);
                if (DateTime.TryParse(input, out newHiringDate)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Formato de fecha inválido.");
                Console.ResetColor();
            }

            decimal newSalary;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("Salario", storedSalary.ToString("G"));
                if (decimal.TryParse(input, out newSalary)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número válido.");
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

            int newDepartmentId;
            while (true)
            {
                string input = ConsoleHelper.ShowValuesToUpdate("ID del Departamento", storedDepartmentId.ToString());
                if (int.TryParse(input, out newDepartmentId)) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Por favor ingrese un número entero válido.");
                Console.ResetColor();
            }

            EmployeeUpdateDto data = new()
            {
                Id = id,
                FirstName = newFirstName,
                LastName = newLastName,
                Email = newEmail,
                HiringDate = newHiringDate,
                Salary = newSalary,
                RoleId = newRoleId,
                DepartmentId = newDepartmentId
            };

            return JsonSerializer.Serialize(data);
        }

        private static string GetEmployeeByIdForm() => JsonSerializer.Serialize(new { Id = AskId(EntitiesEnum.Employee) });

        private static string DeleteEmployeeForm(int? predefinedId) => JsonSerializer.Serialize(new { Id = predefinedId ?? AskId(EntitiesEnum.Employee) });
    }
}