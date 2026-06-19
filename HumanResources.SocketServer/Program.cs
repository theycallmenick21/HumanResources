using HumanResources.SocketServer.Config;
using HumanResources.SocketServer.Network;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;

namespace HumanResources.SocketServer
{
    class Program
    {
        static readonly ServiceCollection services = new();
        static ServiceProvider? serviceProvider;

        static async Task Main(string[] args)
        {
            Console.Title = "SERVIDOR TRANSACCIONAL - RECURSOS HUMANOS";

            services.ConfigureServices();
            serviceProvider = services.BuildServiceProvider();

            TcpListener server = new(IPAddress.Any, 5000);
            server.Start();

            Console.WriteLine("==============================================");
            Console.WriteLine(" SERVIDOR INICIADO - ESPERANDO CONEXIONES...");
            Console.WriteLine("==============================================");

            while (true)
            {
                TcpClient newClient = await server.AcceptTcpClientAsync();

                ClientHandler handler = new(newClient, serviceProvider);
                _ = Task.Run(() => handler.HandleAsync());
            }
        }
    }
}