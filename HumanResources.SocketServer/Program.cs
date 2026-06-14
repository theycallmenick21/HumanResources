using HumanResources.Application.Interfaces;
using HumanResources.SocketServer.Config;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;

namespace HumanResources.SocketServer
{
    class Program
    {
        static Dictionary<string, TcpClient> conectedClients = [];
        static ServiceCollection services = new();
        static ServiceProvider? serviceProvider;
        static ICountryService? countryService;
        static async Task Main(string[] args)
        {
            services.ConfigureServices();
            serviceProvider = services.BuildServiceProvider();
            countryService = serviceProvider.GetRequiredService<ICountryService>();

            await countryService.GetAllAsync();

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

        static async Task HandleClient(TcpClient client)
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

                    if (nombreUsuario != string.Empty)
                        await ProccessRequestAsync(mensaje);
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

        static async Task ProccessRequestAsync(string mensaje)
        {
            string[] parts = mensaje.Split('|', 4);

            string client = parts[0];
            int entity = int.Parse(parts[1]);
            int action = int.Parse(parts[2]);
            string data = parts[3];

            using var scope = serviceProvider!.CreateScope();
            var scopedCountryService = scope.ServiceProvider.GetRequiredService<ICountryService>();

            var x = await scopedCountryService.GetAllAsync();

            Console.WriteLine(x);
        }
    }
}