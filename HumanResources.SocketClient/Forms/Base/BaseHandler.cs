using HumanResources.Domain.Enums;

namespace HumanResources.SocketClient.Forms.Base
{
    public class BaseHandler
    {
        protected static int AskId(EntitiesEnum entidad)
        {
            Console.Write($"\nIngrese el ID del registro de {entidad}: ");
            if (int.TryParse(Console.ReadLine(), out int id))
                return id;
            return 0;
        }
    }
}
