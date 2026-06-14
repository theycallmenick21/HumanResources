namespace HumanResources.SocketClient.Helpers
{
    internal static class ActionHelper
    {
        internal static string ObtainAction(int action) => action switch
        {
            0 => "INSERTAR",
            1 => "ACTUALIZAR",
            2 => "CONSULTAR TODOS",
            3 => "CONSULTAR POR NOMBRE",
            5 => "BORRAR",
            _ => "ACCIÓN DESCONOCIDA"
        };
    }
}
