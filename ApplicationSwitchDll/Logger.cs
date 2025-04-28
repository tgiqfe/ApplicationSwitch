using System.Text;

namespace ApplicationSwitch
{
    internal class Logger
    {
        public static void WriteLine(string message, int indent = 2)
        {
            string now = DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]");
            using (var sw = new StreamWriter(Item.LogFile, true, Encoding.UTF8))
            {
                string indentText = new string(' ', indent);
                Console.WriteLine($"{indentText}{message}");
                sw.WriteLine($"{now} {message}");
            }
        }
    }
}
