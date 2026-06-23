using HumanResources.Domain.Enums;
using HumanResources.Shared.Wrappers;
using HumanResources.SocketClient.Forms;
using HumanResources.SocketClient.Helpers;
using HumanResources.SocketClient.UI;
using System.Net.Sockets;
using System.Text.Json;

namespace HumanResources.SocketClient
{
    class Program
    {
        static TcpClient? client;
        static StreamWriter? writer;
        static StreamReader? reader;
        static string? user;

        static void Main(string[] args)
        {
            Console.Title = "CLIENTE DE RECURSOS HUMANOS";

            Console.Write("Ingresa IP del Servidor: ");
            string serverIP = Console.ReadLine()!;
            Console.Write("Ingresa el puerto de la conexión: ");
            string serverPort = Console.ReadLine()!;
            Console.Write("Ingresa tu nombre de usuario: ");
            user = Console.ReadLine()!;

            try
            {
                client = new TcpClient(serverIP, int.Parse(serverPort));
                NetworkStream stream = client.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);

                writer.WriteLine(user);
                Console.CursorVisible = false;

                ExecuteMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR DE RED]: {ex.Message}");
            }
            finally
            {
                client?.Close();
            }
        }

        static void ExecuteMainMenu()
        {
            bool exit = false;
            string[] mainMenuOptions =
            [
                "CIUDAD",
                "PAÍS",
                "DEPARTAMENTO",
                "EMPLEADO",
                "UBICACIÓN",
                "REGISTRO",
                "ROL",
                "Salir del Sistema"
            ];

            while (!exit)
            {
                int selectedIndex = MenuManager.ShowMenu("MENÚ PRINCIPAL", mainMenuOptions, true);

                if (selectedIndex == mainMenuOptions.Length - 1)
                {
                    exit = true;
                    Console.Clear();
                    Console.WriteLine("Saliendo del sistema...");
                }
                else if (selectedIndex != -1)
                {
                    EntitiesEnum selectedEntity = (EntitiesEnum)selectedIndex;
                    HandleSubMenu(selectedEntity);
                }
            }
        }

        static void HandleSubMenu(EntitiesEnum entidad)
        {
            bool back = false;
            string entityName = entidad switch
            {
                EntitiesEnum.City => "CIUDAD",
                EntitiesEnum.Country => "PAÍS",
                EntitiesEnum.Department => "DEPARTAMENTO",
                EntitiesEnum.Employee => "EMPLEADO",
                EntitiesEnum.Location => "UBICACIÓN",
                EntitiesEnum.Record => "REGISTRO",
                EntitiesEnum.Role => "ROL",
                _ => string.Empty
            };

            string[] options = [
                $"[+] Insertar Nuevo {entityName}",
                $"[*] Consultar Todos los registros en {entityName}",
                $"[?] Consultar {entityName} por ID"
            ];

            while (!back)
            {
                int selection = MenuManager.ShowMenu($"GESTIÓN DE {entityName.ToUpper()}", options, false);

                if (selection == -1)
                {
                    back = true;
                }
                else
                {
                    int realAction = selection switch
                    {
                        0 => 0,
                        1 => 2,
                        2 => 3,
                        _ => 2
                    };

                    ProcessTransaction(entidad, realAction);
                }
            }
        }

        static void ProcessTransaction(EntitiesEnum entity, int action, int? predefinedId = null, JsonElement? dbData = null)
        {
            Console.Clear();
            Console.CursorVisible = true;

            string actionStr = action switch
            {
                0 => "CREAR",
                1 => "ACTUALIZAR",
                2 => "CONSULTAR TODOS LOS REGISTROS DE",
                3 => "CONSULTAR POR ID",
                5 => "BORRAR",
                _ => ""
            };

            Console.WriteLine($"--- {actionStr} {entity.ToString().ToUpper()} ---\n");

            string payloadJson = "";

            if (action != 2)
                payloadJson = FormHandler.ProccessForm(entity, action, predefinedId, dbData);

            string finalPackagedData = $"{user}|{(int)entity}|{action}|{payloadJson}";
            Console.WriteLine("\n[Enviando petición al servidor...]");
            writer?.WriteLine(finalPackagedData);

            try
            {
                string jsonResponse = reader?.ReadLine()!;

                Response<JsonElement>? response = JsonSerializer.Deserialize<Response<JsonElement>>(jsonResponse) ?? throw new Exception("Respuesta nula del servidor.");
                Console.Clear();

                if (!response.IsSuccess)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[ERROR DEL SERVIDOR]: {response.Message}");
                    Console.ResetColor();

                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        foreach (string error in response.Errors)
                        {
                            Console.WriteLine($" - {error}");
                        }
                    }
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey(true);
                    return;
                }

                switch (action)
                {
                    case 2:
                        ShowInteractiveList(entity, response.Data.ToString());
                        break;
                    case 3:
                        ShowInteractiveList(entity, response.Data.ToString(), true);
                        break;
                    case 0:
                    case 1:
                    case 5:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n[ÉXITO]: {response.Message}");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey(true);
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Error de Procesamiento]: {ex.Message}");
                Console.ReadKey(true);
            }

            Console.CursorVisible = false;
        }

        static void ShowInteractiveList(EntitiesEnum entity, string jsonResponse, bool onlyOne = false)
        {
            try
            {
                Console.CursorVisible = false;

                List<JsonElement>? records = onlyOne
                    ? [JsonSerializer.Deserialize<JsonElement>(jsonResponse)]
                    : JsonSerializer.Deserialize<List<JsonElement>>(jsonResponse);

                if (records == null || records.Count == 0)
                {
                    Console.WriteLine("\nLa base de datos no devolvió registros.");
                    Console.ReadKey();
                    return;
                }

                bool exit = false;
                while (!exit)
                {
                    string[] listOptions = new string[records.Count];

                    for (int i = 0; i < records.Count; i++)
                    {
                        listOptions[i] = JsonExtractorHelper.ExtractDisplayString(entity, records[i]);
                    }

                    int selectedOption = MenuManager.ShowMenu($"REGISTROS DE {entity.ToString().ToUpper()}", listOptions);

                    if (selectedOption == -1)
                    {
                        exit = true;
                    }
                    else
                    {
                        int selectedId = JsonExtractorHelper.ExtractRealId(entity, records[selectedOption]);
                        ShowItemActionsMenu(entity, selectedId, listOptions[selectedOption], records[selectedOption]);
                        exit = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Error procesando JSON]: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void ShowItemActionsMenu(EntitiesEnum entity, int recordId, string displayString, JsonElement? dbData = null)
        {
            string[] options = [
                "[~] Actualizar este registro",
                "[x] Borrar este registro",
            ];

            if (entity == EntitiesEnum.Role)
                options = [.. options.ExceptBy(["[x] Borrar este registro"], o => o)];

            int selection = MenuManager.ShowMenu($"ACCIÓN PARA: {displayString}", options);

            ProcessTransaction(entity, selection == 0 ? 1 : 5, recordId, dbData);
        }
    }
}