using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using ApplicationSwitch.Sample.SampleRule;
using System.Reflection;

namespace ApplicationSwitch
{
    public class Switcher
    {
        public static void CreateSample(int num)
        {
            string text = num switch
            {
                1 => Rule_File01.Create(),
                2 => Rule_File02.Create(),
                _ => "",
            };

            Console.WriteLine(text);
        }

        public static string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
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
