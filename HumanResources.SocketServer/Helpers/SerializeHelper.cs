using HumanResources.Shared.Wrappers;
using System.Text.Json;

namespace HumanResources.SocketServer.Helpers
{
    public class SerializeHelper
    {
        internal static string SerializeFail(string message)
        {
            return JsonSerializer.Serialize(Response<string>.Fail(message));
        }
    }
}
