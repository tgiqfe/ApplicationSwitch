
using System.Diagnostics;

namespace ApplicationSwitch
{
    public class Setting
    {
        internal readonly static string WorkDirectory =
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        #region LogFile parameter

        internal static string LogFile = Path.Combine(
            WorkDirectory, _LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");

        private static string _LogDirectory = "Logs";
        public static string LogDirectory
        {
            get { return _LogDirectory; }
            set
            {
                _LogDirectory = value;
                LogFile = Path.Combine(
                    WorkDirectory, _LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");
            }
        }

        #endregion
        
        public static string EvacuateDirectory = "Evacuate";
        public static string RulesDirectory = "Rules";
        public static bool EvacuateDirectoryHidden = true;
        public static bool WordkDirectoryHidden = true;
    }
}
