using System.Diagnostics;

namespace ApplicationSwitch.Lib
{
    internal class Item
    {
        internal static string WorkDirectory { get; private set; }
        internal static string LogDirectory { get; private set; }
        internal static string LogFile { get; private set; }
        internal static string EvacuateDirectory { get; private set; }
        internal static string RulesDirectory { get; private set; }
        internal static bool EvacuateDirectoryHidden { get; private set; }
        internal static bool WorkDirectoryHidden { get; private set; }

        public static void Initialize()
        {
            WorkDirectory = Setting.WorkDirectory ??
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            LogDirectory = Path.Combine(WorkDirectory, Setting.LogDirectory);
            LogFile = Path.Combine(LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");
            EvacuateDirectory = Path.Combine(WorkDirectory, Setting.EvacuateDirectory);
            RulesDirectory = Path.Combine(WorkDirectory, Setting.RulesDirectory);
            EvacuateDirectoryHidden = Functions.IsEnable(Setting.EvacuateDirectoryHidden);
            WorkDirectoryHidden = Functions.IsEnable(Setting.WordkDirectoryHidden);
        }
    }
}
