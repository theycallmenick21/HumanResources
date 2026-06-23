namespace HumanResources.SocketClient.Helpers
{
    public class ConsoleHelper
    {
        public static string ShowValuesToUpdate(string message, string actualValue)
        {
            Console.Write($"{message} [{actualValue}]: ");
            string entry = Console.ReadLine()!;

            return string.IsNullOrWhiteSpace(entry) ? actualValue : entry;
        }
    }
}
