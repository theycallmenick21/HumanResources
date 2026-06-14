using HumanResources.Domain.Enums;
using HumanResources.SocketClient.Helpers;
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

                Console.Title = $"CLIENTE {user} - SISTEMA TRANSACCIONAL";
                Console.CursorVisible = false;

                bool exit = false;
                string[] entityNames = Enum.GetNames<EntitiesEnum>();
                string[] mainMenuOptions = [.. entityNames, "Salir del Sistema"];

                while (!exit)
                {
                    int selectedEntity = ShowMenu("=== MENÚ PRINCIPAL: SELECCIONE UNA ENTIDAD ===", mainMenuOptions);

                    if (selectedEntity == mainMenuOptions.Length - 1)
                    {
                        exit = true;
                        Console.Clear();
                        Console.WriteLine("Saliendo del sistema...");
                    }
                    else
                    {
                        EntitiesEnum entidadSeleccionada = (EntitiesEnum)selectedEntity;
                        HandleSubMenuCRUD(entidadSeleccionada);
                    }
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
            }
        }

        static int ShowMenu(string title, string[] options)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(title + "\n");
                Console.WriteLine("Use las flechas ARRIBA/ABAJO para navegar y ENTER para seleccionar.\n");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine($" > {options[i]} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"   {options[i]} ");
                    }
                }

                ConsoleKeyInfo tecla = Console.ReadKey(true);

                switch (tecla.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0) selectedIndex = options.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex >= options.Length) selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        return selectedIndex;
                }
            }
        }

        static void HandleSubMenuCRUD(EntitiesEnum entidad)
        {
            bool backToMainMenu = false;
            string EntityName = entidad.ToString();

            string[] options = [
                $"Insertar en {EntityName}",
                $"Consultar Todos en {EntityName}",
                $"Consultar por ID en {EntityName}",
                "Volver al menú principal"
            ];

            while (!backToMainMenu)
            {
                int seleccion = ShowMenu($"--- GESTIÓN DE {EntityName.ToUpper()} ---", options);

                if (seleccion == options.Length - 1)
                {
                    backToMainMenu = true;
                }
                else
                {
                    int accionReal = seleccion == 0 ? 0 : 2;
                    ExecuteAction(entidad, accionReal);
                }
            }
        }

        static void ExecuteAction(EntitiesEnum entity, int action, int? predefinedId = null)
        {
            Console.Clear();
            Console.CursorVisible = true;

            string username = user;
            string actionName = ActionHelper.ObtainAction(action);
            string entityName = entity.ToString().ToUpper();

            Console.WriteLine($"--- {actionName} {entityName} ---");

            string data = "";

            if (action != 2)
                data = EnrutarFormulario(entity, action, predefinedId);

            string packedData = $"{username}|{(int)entity}|{action}|{data}";
            Console.WriteLine("\nEnviando transacción al servidor...");
            writer?.WriteLine(packedData);

            try
            {
                string respuestaServidor = reader?.ReadLine()!;

                if (action == 2)
                {
                    MostrarListaInteractiva(entity, respuestaServidor);
                }
                else
                {
                    Console.WriteLine("\n[Respuesta del Servidor]:\n" + respuestaServidor);
                    Console.WriteLine("\nPresione cualquier tecla para volver al submenú...");
                    Console.ReadKey(true);
                }
            }
            catch
            {
                Console.WriteLine("\n[Error]: Se perdió la conexión o el servidor no respondió correctamente.");
                Console.ReadKey(true);
            }

            Console.CursorVisible = false;
        }

        static void MostrarListaInteractiva(EntitiesEnum entidad, string jsonResponse)
        {
            try
            {
                var registros = JsonSerializer.Deserialize<List<JsonElement>>(jsonResponse);

                if (registros == null || registros.Count == 0)
                {
                    Console.WriteLine("\nNo hay registros en la base de datos para mostrar.");
                    Console.ReadKey();
                    return;
                }

                bool salir = false;
                while (!salir)
                {
                    // Construimos las opciones del menú con los nombres de los registros
                    string[] opcionesLista = new string[registros.Count + 1];
                    for (int i = 0; i < registros.Count; i++)
                    {
                        opcionesLista[i] = ExtraerDisplayString(entidad, registros[i]);
                    }
                    opcionesLista[registros.Count] = "Volver al submenú anterior";

                    int seleccion = ShowMenu($"--- REGISTROS DE {entidad.ToString().ToUpper()} ---", opcionesLista);

                    if (seleccion == registros.Count) // Eligió Volver
                    {
                        salir = true;
                    }
                    else
                    {
                        // Extraemos el ID real del registro seleccionado
                        int idSeleccionado = ExtraerIdReal(entidad, registros[seleccion]);
                        string nombreSeleccionado = opcionesLista[seleccion];

                        // Abrimos el submenú de edición/borrado para ese registro específico
                        SubMenuRegistroEspecifico(entidad, idSeleccionado, nombreSeleccionado);

                        // Después de editar o borrar, lo ideal es salir para recargar la lista
                        salir = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n[Error procesando la lista]: " + ex.Message);
                Console.WriteLine("Asegúrese de que el servidor esté devolviendo un JSON válido. Respuesta recibida: " + jsonResponse);
                Console.ReadKey();
            }
        }

        static void SubMenuRegistroEspecifico(EntitiesEnum entidad, int idRegistro, string displayString)
        {
            string[] opciones = [
                "1. Actualizar este registro",
                "2. Borrar este registro",
                "3. Cancelar"
            ];

            int seleccion = ShowMenu($"--- OPCIONES PARA: {displayString} ---", opciones);

            if (seleccion == 0)
            {
                ExecuteAction(entidad, 1, idRegistro);
            }
            else if (seleccion == 1)
            {
                ExecuteAction(entidad, 5, idRegistro);
            }
        }

        // --- EXTRACTORES DINÁMICOS DE JSON ---

        static int ExtraerIdReal(EntitiesEnum entidad, JsonElement item)
        {
            // Dependiendo de la entidad, busca el nombre exacto de la columna de ID
            return entidad switch
            {
                EntitiesEnum.Country => item.GetProperty("IdPais").GetInt32(),
                EntitiesEnum.Department => item.GetProperty("IdDepartamento").GetInt32(),
                _ => item.GetProperty("Id").GetInt32() // Valor por defecto
            };
        }

        static string ExtraerDisplayString(EntitiesEnum entidad, JsonElement item)
        {
            // Dependiendo de la entidad, arma un string bonito para mostrar en el menú
            return entidad switch
            {
                EntitiesEnum.Country => $"ID: {item.GetProperty("IdPais")} | País: {item.GetProperty("NombrePais")}",
                EntitiesEnum.Department => $"ID: {item.GetProperty("IdDepartamento")} | Depto: {item.GetProperty("NombreDepartamento")}",
                _ => item.ToString()
            };
        }

        // --- FORMULARIOS ACTUALIZADOS ---

        // Nota: Ahora reciben un int? idPredefinido
        static string EnrutarFormulario(EntitiesEnum entidad, int accion, int? idPredefinido)
        {
            // Si es BORRAR (5) o CONSULTAR POR ID (3)
            if (accion == 3 || accion == 5)
            {
                // Si ya tenemos el ID del menú interactivo, lo usamos directamente
                int id = idPredefinido ?? SolicitarId(entidad, accion);
                return $"{{\"Id\": {id}}}";
            }

            return entidad switch
            {
                EntitiesEnum.Country => CapturarFormularioPais(accion, idPredefinido),
                EntitiesEnum.Department => CapturarFormularioDepartamento(accion, idPredefinido),
                _ => throw new NotImplementedException($"El formulario para {entidad} no está listo.")
            };
        }

        static int SolicitarId(EntitiesEnum entidad, int accion)
        {
            Console.Write($"Ingrese el ID del {entidad} a {ActionHelper.ObtainAction(accion).ToLower()}: ");
            return int.Parse(Console.ReadLine()!);
        }

        static string CapturarFormularioPais(int accion, int? idPredefinido)
        {
            int idPais;

            if (idPredefinido.HasValue)
            {
                idPais = idPredefinido.Value;
                Console.WriteLine($"ID del País: {idPais} (Autoseleccionado)");
            }
            else
            {
                Console.Write("Ingrese el ID del País: ");
                idPais = int.Parse(Console.ReadLine()!);
            }

            Console.Write("Ingrese el Nombre del País: ");
            string nombrePais = Console.ReadLine()!;

            var pais = new { IdPais = idPais, NombrePais = nombrePais };
            return JsonSerializer.Serialize(pais);
        }

        static string CapturarFormularioDepartamento(int accion, int? idPredefinido)
        {
            int idDepto;

            if (idPredefinido.HasValue)
            {
                idDepto = idPredefinido.Value;
                Console.WriteLine($"ID del Departamento: {idDepto} (Autoseleccionado)");
            }
            else
            {
                Console.Write("Ingrese el ID del Departamento: ");
                idDepto = int.Parse(Console.ReadLine()!);
            }

            Console.Write("Ingrese el Nombre del Departamento: ");
            string nombre = Console.ReadLine()!;

            Console.Write("Ingrese el Presupuesto: ");
            float presupuesto = float.Parse(Console.ReadLine()!);

            var departamento = new { IdDepartamento = idDepto, NombreDepartamento = nombre, Presupuesto = presupuesto };
            return JsonSerializer.Serialize(departamento);
        }
    }
}