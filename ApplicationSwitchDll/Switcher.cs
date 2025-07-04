﻿using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using ApplicationSwitch.Sample.SampleRule;
using System.Reflection;

namespace ApplicationSwitch
{
    public class Switcher
    {
        /// <summary>
        /// Create sample rule file.
        /// </summary>
        /// <param name="num"></param>
        public static void CreateSample(int num)
        {
            string text = num switch
            {
                1 => Rule_File01.Create(),
                2 => Rule_File02.Create(),
                3 => Rule_File03.Create(),
                4 => Rule_Registry01.Create(),
                5 => Rule_Command01.Create(),
                6 => Rule_Hidden01.Create(),
                _ => "",
            };

            Console.WriteLine(text);
        }

        /// <summary>
        /// Show version.
        /// </summary>
        public static string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Create instance.
        /// this program is start!
        /// </summary>
        /// <returns></returns>
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
