using HumanResources.SocketServer.Routing;
using System.Net.Sockets;

namespace HumanResources.SocketServer.Network
{
    public class ClientHandler(TcpClient client, IServiceProvider serviceProvider)
    {
        private readonly TcpClient _client = client;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task HandleAsync()
        {
            await using NetworkStream stream = _client.GetStream();
            using StreamReader reader = new(stream);
            await using StreamWriter writer = new(stream) { AutoFlush = true };

            string? userName = "";

            try
            {
                userName = await reader.ReadLineAsync();
                Console.WriteLine($"[LOG] Usuario {userName} conectado.");

                while (true)
                {
                    string? message = await reader.ReadLineAsync();

                    if (string.IsNullOrEmpty(message)) break;

                    Console.WriteLine($"[REQ de {userName}]: Procesando petición...");

                    string response = await RequestRouter.RouteRequestAsync(message, _serviceProvider);

                    await writer.WriteLineAsync(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Desconexión abrupta de {userName}: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"[LOG] {userName} se ha desconectado.");
                _client.Close();
            }
        }
    }
}