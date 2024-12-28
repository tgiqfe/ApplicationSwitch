using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleBase
    {
        public string Name { get; set; }

        public string EvacuatePath { get; set; }

        public virtual void EnableProcess() { }

        public virtual void DisableProcess() { }
    }
}
