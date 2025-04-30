using ApplicationSwitch.Lib;
using ApplicationSwitch.Sample.SampleRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ApplicationSwitch.Lib.Rules;

namespace ApplicationSwitch
{
    public class Switcher
    {
        public static void CreateSample(int num)
        {
            switch (num)
            {
                case 1: Console.WriteLine(Rule_File01.Create()); break;
                case 2: Console.WriteLine(Rule_File02.Create()); break;
                case 3: Console.WriteLine(Rule_File03.Create()); break;
                case 4: Console.WriteLine(Rule_Registry01.Create()); break;
                case 5: Console.WriteLine(Rule_Command01.Create()); break;
                case 6: Console.WriteLine(Rule_Hidden01.Create()); break;
                default: Console.WriteLine(""); break;
            }
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
