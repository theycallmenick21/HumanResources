namespace HumanResources.SocketClient.UI
{
    public static class MenuManager
    {
        public static int ShowMenu(string title, string[] options)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=================================================");
                Console.WriteLine($" {title.ToUpper()}");
                Console.WriteLine("=================================================\n");
                Console.WriteLine("Use las flechas ARRIBA/ABAJO para navegar y ENTER para seleccionar y la tecla de RETROCESO para volver al menú anterior.\n");

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
                    case ConsoleKey.Backspace:
                        return -1;
                }
            }
        }
    }
}