using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib
{
    internal class Logger
    {
        private static string LogFile = null;

        public static void WriteLine(string message, int indent = 2)
        {
            LogFile ??= PrepareLogFilePath();

            string now = DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]");
            using (var sw = new StreamWriter(LogFile, true, Encoding.UTF8))
            {
                string indentText = new string(' ', indent);
                Console.WriteLine($"{indentText}{message}");
                sw.WriteLine($"{now} {message}");
            }
        }

        private static string PrepareLogFilePath()
        {
            string logDir1 = Path.Combine(
                 Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                 "Logs");
            string logDir2 = Path.Combine(
                Environment.ExpandEnvironmentVariables("%TEMP%"),
                "Logs");
            string logFile1 = Path.Combine(logDir1, "log.txt");
            string logFile2 = Path.Combine(logDir2, "log.txt");

            try
            {
                if (!Directory.Exists(logDir1))
                {
                    Directory.CreateDirectory(logDir1);
                }
                File.CreateText(logFile1);
                File.Delete(logFile1);
                return logFile1;
            }
            catch
            {
                if (!Directory.Exists(logDir2))
                {
                    Directory.CreateDirectory(logDir2);
                }
                return logFile2;
            }
        }
    }
}
