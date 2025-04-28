using System.Diagnostics;

namespace ApplicationSwitch
{
    internal class Item
    {
        private readonly string[] candidate_enable = 
            new string[] { "Enable", "enable", "on", "true", "1", "true", "tru", "$true" };
        private readonly string[] candidate_disable =
            new string[] { "Disable", "disable", "off", "false", "0", "false", "fals", "$false" };

        internal string WorkDirectory { get; private set; }
        internal static string LogFile { get; private set; }
        internal static string EvacuateDirectory { get; private set; }

        public void Initialize()
        {
            WorkDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            LogFile = Path.Combine(WorkDirectory, Setting.LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");
            EvacuateDirectory = Path.Combine(WorkDirectory, Setting.EvacuateDirectory);
        }


    }
}
