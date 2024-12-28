using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleFile : RuleBase
    {
        public string Target { get; set; }

        private string _evacuatePath { get { return Path.Combine(this.EvacuatePath, Name); } }

        public override void EnableProcess()
        {

        }

        public override void DisableProcess()
        {
            if (File.Exists(this.Target))
            {

            }
            else if (Directory.Exists(this.Target))
            {

            }
        }
    }
}
