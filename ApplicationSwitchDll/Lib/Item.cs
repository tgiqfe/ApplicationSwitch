using System.Diagnostics;

namespace ApplicationSwitch.Lib
{
    internal class Item
    {
        internal string WorkDirectory { get; private set; }
        internal static string LogFile { get; private set; }
        internal static string EvacuateDirectory { get; private set; }
        internal static string RulesDirectory { get; private set; }
        internal static bool EvacuateDirectoryHidden { get; private set; }
        internal static bool WorkDirectoryHidden { get; private set; }

        public void Initialize()
        {
            WorkDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            LogFile = Path.Combine(WorkDirectory, Setting.LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");
            EvacuateDirectory = Path.Combine(WorkDirectory, Setting.EvacuateDirectory);
            RulesDirectory = Path.Combine(WorkDirectory, Setting.RulesDirectory);
            EvacuateDirectoryHidden = Functions.IsEnable(Setting.EvacuateDirectoryHidden);
            WorkDirectoryHidden = Functions.IsEnable(Setting.WordkDirectoryHidden);
        }
    }
}
