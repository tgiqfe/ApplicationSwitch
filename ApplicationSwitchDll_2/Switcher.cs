using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using System.Runtime.Versioning;

namespace ApplicationSwitch
{
    [SupportedOSPlatform("windows")]
    public class Switcher
    {
        #region Common method

        private static void InitialCommon()
        {
            Item.Initialize();

            if (!Directory.Exists(Item.LogDirectory))
            {
                Directory.CreateDirectory(Item.LogDirectory);
            }
            Logger.WriteLine("Start Application switch.", 0);
        }

        private static void FinishCommon()
        {
            Logger.WriteLine("End Application switch.", 0);
        }

        #endregion

        public static void CreateSample_RuleFile()
        {
            var appRoot = Sample.CreateSample.RuleFile01.Create();
            Functions.Show(appRoot);
        }

        public static void CreateSample_RuleRegistry()
        {

        }

        public static void CreateSample_RuleCommand()
        {

        }

        public static void CreateSample_RuleHidden()
        {

        }

        public static List<AppRoot> Load()
        {
            InitialCommon();
            var ret = AppRoot.LoadRuleFiles(Item.RulesDirectory);
            FinishCommon();

            return ret;
        }

    }
}
