using ApplicationSwitch;
using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using ApplicationSwitch.Sample.SampleRule;
using System.Security.Cryptography.X509Certificates;

namespace ApplicationSwitch
{
    public class Switcher
    {
        public static void CreateSample()
        {
            string text = Rule_File01.Create();
            Console.WriteLine(text);
        }

        public static Switcher GetInstance()
        {
            return new Switcher();
        }

        public AppRoot LoadRuleFile(string path)
        {
            return File.Exists(path) ? Functions.Load<AppRoot>(path) : null;
        }


    }
}
