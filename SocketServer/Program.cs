using HumanResources.Models.Models;
using HumanResources.Services.Services;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace HumanResources.SocketServer
{
    class Program
    {
        static Dictionary<string, TcpClient> conectedClients = [];

        static async Task Main(string[] args)
        {
            Console.Title = "SERVIDOR DE CHAT BIDIRECCIONAL";

            TcpListener server = new(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("Servidor iniciado y contestando OK. Esperando conexiones...");

            while (true)
            {
                TcpClient newClient = await server.AcceptTcpClientAsync();

                _ = Task.Run(() => HandleClient(newClient));
            }
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new(stream);

            string nombreUsuario = "";

            try
            {
                nombreUsuario = reader.ReadLine()!;
                Console.WriteLine($"[LOG] Usuario {nombreUsuario} conectado.");

                lock (conectedClients)
                {
                    conectedClients.Add(nombreUsuario, client);
                }

                while (true)
                {
                    string mensaje = reader.ReadLine()!;

                    if(nombreUsuario != string.Empty)
                        ProccessRequest(mensaje);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
            }
            finally
            {
                lock (conectedClients)
                {
                    conectedClients.Remove(nombreUsuario);
                }

                Console.WriteLine($"[LOG] {nombreUsuario} se ha desconectado.");
                client.Close();
            }
        }

        static void ProccessRequest(string mensaje)
        {
            string[] parts = mensaje.Split('|', 4);

            string client = parts[0];
            int entity = int.Parse(parts[1]);
            int action = int.Parse(parts[2]);
            string data = parts[3];

            PaisService paisService = new();

            paisService.GetAllAsync().ContinueWith(task =>
            {
                List<Country> paises = task.Result;

                string jsonResponse = JsonSerializer.Serialize(paises);

                StreamWriter writer = new(conectedClients.First(c => c.Key == client).Value.GetStream()) { AutoFlush = true };
                writer.WriteLine(jsonResponse);

            });
        }
    }
}