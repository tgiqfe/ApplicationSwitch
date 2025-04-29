using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch
{
    public class Setting
    {
        public string WorkDirectory { get; set; }
        public string EvacuateDirectory { get; set; }
        public string RulesDirectory { get; set; }
        public string EvacuateDirectoryHidden { get; set; }
        public string WorkDirectoryHidden { get; set; }

        public static Setting GetInstance()
        {
            return new Setting()
            {
                WorkDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                EvacuateDirectory = "Evacuate",
                RulesDirectory = "Rules",
                EvacuateDirectoryHidden = "true",
                WorkDirectoryHidden = "true"
            };
        }
    }
}
