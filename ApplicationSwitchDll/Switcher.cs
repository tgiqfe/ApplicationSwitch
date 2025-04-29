using ApplicationSwitch;
using ApplicationSwitch.Lib;
using ApplicationSwitch.Sample.SampleRule;
using System.Security.Cryptography.X509Certificates;

namespace ApplicationSwitch
{
    public class Switcher
    {
        private static void StartCommon(Setting setting)
        {
            Item.LogFile =
                Path.Combine(setting.LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");
        }

        private static void EndCommon(Setting setting)
        {
        }

        public static void Show(Setting setting)
        {
            StartCommon(setting);


            EndCommon(setting);
        }

        public static void CreateSample()
        {
            string text = Rule_File01.Create();
            Console.WriteLine(text);
        }
    }
}
