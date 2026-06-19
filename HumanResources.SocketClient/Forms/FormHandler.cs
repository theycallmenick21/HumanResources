using HumanResources.Domain.Enums;
using System.Text.Json;

namespace HumanResources.SocketClient.Forms
{
    public static class FormHandler
    {
        public static string CapturarDatos(EntitiesEnum entidad, int accion, int? idPredefinido = null)
        {
            if (accion == 3 || accion == 5)
            {
                int id = idPredefinido ?? askId(entidad);
                return $"{{\"Id\": {id}}}";
            }

            return entidad switch
            {
                EntitiesEnum.City => GetCity(idPredefinido),
                EntitiesEnum.Country => GetCountry(idPredefinido),
                EntitiesEnum.Department => GetDepartment(idPredefinido),
                EntitiesEnum.Employee => GetEmployee(idPredefinido),
                EntitiesEnum.Location => GetLocation(idPredefinido),
                EntitiesEnum.Record => GetRecord(idPredefinido),
                EntitiesEnum.Role => GetRole(idPredefinido),
                _ => throw new NotImplementedException($"El formulario para {entidad} no está implementado.")
            };
        }

        private static int askId(EntitiesEnum entidad)
        {
            Console.Write($"\nIngrese el ID del registro de {entidad}: ");
            if (int.TryParse(Console.ReadLine(), out int id))
                return id;

            return 0;
        }

        private static string GetCity(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.City);

            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: id, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }

        private static string GetCountry(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.Country);

            Console.Write("Ingrese el Nombre del País: ");
            string nombre = Console.ReadLine()!;

            (int Id, string Name) data = (Id: id, Name: nombre);
            return JsonSerializer.Serialize(data);
        }

        private static string GetDepartment(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.Department);

            Console.Write("Ingrese el Nombre del Departamento: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID de la Locación asociada: ");
            int idLocacion = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int LocationId) data = (Id: id, Name: nombre, LocationId: idLocacion);
            return JsonSerializer.Serialize(data);
        }

        private static string GetEmployee(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.City);

            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: id, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }

        private static string GetLocation(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.City);

            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: id, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }

        private static string GetRecord(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.City);

            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: id, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }

        private static string GetRole(int? idPredefinido)
        {
            int id = idPredefinido ?? askId(EntitiesEnum.City);

            Console.Write("Ingrese el Nombre de la Ciudad: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el ID del País asociado: ");
            int idPais = int.Parse(Console.ReadLine()!);

            (int Id, string Name, int CountryId) data = (Id: id, Name: nombre, CountryId: idPais);
            return JsonSerializer.Serialize(data);
        }

    }
}