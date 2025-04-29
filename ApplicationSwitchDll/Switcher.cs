using ApplicationSwitch;
using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using ApplicationSwitch.Sample.SampleRule;
using System.Security.Cryptography.X509Certificates;
using YamlDotNet.Serialization;

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

        /// <summary>
        /// Evacuate Directory
        /// </summary>
        public string EvacuateDirectory
        {
            get { return Item.EvacuateDirectory; }
            set { Item.EvacuateDirectory = value; }
        }

        /// <summary>
        /// Evacuate directory hidden/not hidden.
        /// </summary>
        public bool HiddenEvacuateDirectory
        {
            get { return Item.HiddenEvacuateDirectory; }
            set { Item.HiddenEvacuateDirectory = value; }
        }

        /// <summary>
        /// Load from rule file. return AppRoot object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AppRoot LoadRuleFile(string path)
        {
            return File.Exists(path) ? Functions.Load<AppRoot>(path) : null;
        }
    }
}
