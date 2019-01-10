using System;
using System.Text;

namespace org.newpointe.util
{
    public static class ConsoleUtil
    {

        /// Gets input from the console without echoing it back.
        /// SEE: https://stackoverflow.com/a/49159983
        public static string ReadLine_Hidden()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            return input.ToString();
        }
    }
}