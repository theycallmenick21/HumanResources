using HumanResources.Domain.Enums;
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
            string[] entityNames = Enum.GetNames<EntitiesEnum>();
            string[] mainMenuOptions = [.. entityNames, "Salir del Sistema"];

            while (!exit)
            {
                int selectedEntity = MenuManager.ShowMenu("MENÚ PRINCIPAL", mainMenuOptions);

                if (selectedEntity == mainMenuOptions.Length - 1)
                {
                    exit = true;
                    Console.Clear();
                    Console.WriteLine("Saliendo del sistema...");
                }
                else if (selectedEntity != -1)
                {
                    EntitiesEnum entidadSeleccionada = (EntitiesEnum)selectedEntity;
                    HandleSubMenu(entidadSeleccionada);
                }
            }
        }

        static void HandleSubMenu(EntitiesEnum entidad)
        {
            bool back = false;
            string entityName = entidad.ToString();

            string[] options = [
                $"[+] Insertar Nuevo {entityName}",
                $"[*] Consultar Todos los {entityName}s",
                $"[?] Consultar {entityName} por ID",
                "[<] Volver al menú principal"
            ];

            while (!back)
            {
                int seleccion = MenuManager.ShowMenu($"GESTIÓN DE {entityName.ToUpper()}", options);

                if (seleccion == -1)
                {
                    back = true;
                }
                else
                {
                    int accionReal = seleccion switch
                    {
                        0 => 0,
                        1 => 2,
                        2 => 3,
                        _ => 2
                    };

                    ProcesarTransaccion(entidad, accionReal);
                }
            }
        }

        static void ProcesarTransaccion(EntitiesEnum entidad, int accion, int? idPredefinido = null)
        {
            Console.Clear();
            Console.CursorVisible = true;

            string actionStr = accion == 0 ? "CREAR" : accion == 2 ? "CONSULTAR TODOS" : accion == 3 ? "CONSULTAR ID" : accion == 5 ? "BORRAR" : "ACTUALIZAR";
            Console.WriteLine($"--- {actionStr} {entidad.ToString().ToUpper()} ---\n");

            string payloadJson = "";
            if (accion != 2)
            {
                payloadJson = FormHandler.CapturarDatos(entidad, accion, idPredefinido);
            }

            string paqueteFinal = $"{user}|{(int)entidad}|{accion}|{payloadJson}";
            Console.WriteLine("\n[Enviando petición al servidor...]");
            writer?.WriteLine(paqueteFinal);

            try
            {
                string respuesta = reader?.ReadLine()!;

                if (accion == 2)
                {
                    MostrarListaInteractiva(entidad, respuesta);
                }
                else
                {
                    Console.WriteLine("\n[Respuesta del Servidor]:\n" + respuesta);
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey(true);
                }
            }
            catch
            {
                Console.WriteLine("\n[Error]: Se perdió la conexión con el servidor.");
                Console.ReadKey(true);
            }

            Console.CursorVisible = false;
        }

        static void MostrarListaInteractiva(EntitiesEnum entidad, string jsonResponse)
        {
            try
            {
                List<JsonElement>? registros = JsonSerializer.Deserialize<List<JsonElement>>(jsonResponse);

                if (registros == null || registros.Count == 0)
                {
                    Console.WriteLine("\nLa base de datos no devolvió registros.");
                    Console.ReadKey();
                    return;
                }

                bool salir = false;
                while (!salir)
                {
                    string[] opcionesLista = new string[registros.Count + 1];
                    for (int i = 0; i < registros.Count; i++)
                    {
                        opcionesLista[i] = JsonExtractor.ExtraerDisplayString(entidad, registros[i]);
                    }
                    opcionesLista[registros.Count] = "[<] Volver atrás";

                    int seleccion = MenuManager.ShowMenu($"REGISTROS DE {entidad.ToString().ToUpper()}", opcionesLista);

                    if (seleccion == registros.Count)
                    {
                        salir = true;
                    }
                    else
                    {
                        int idSeleccionado = JsonExtractor.ExtraerIdReal(entidad, registros[seleccion]);
                        SubMenuAccionesRegistro(entidad, idSeleccionado, opcionesLista[seleccion]);
                        salir = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Error procesando JSON]: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void SubMenuAccionesRegistro(EntitiesEnum entidad, int idRegistro, string displayString)
        {
            string[] opciones = [
                "[✎] Actualizar este registro",
                "[x] Borrar este registro",
                "[<] Cancelar"
            ];

            int seleccion = MenuManager.ShowMenu($"ACCIÓN PARA: {displayString}", opciones);

            if (seleccion == 0)
            {
                ProcesarTransaccion(entidad, 1, idRegistro);
            }
            else if (seleccion == 1)
            {
                ProcesarTransaccion(entidad, 5, idRegistro);
            }
        }
    }
}