using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleRegistry : RuleBase
    {
        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }
        public object RegistryValue { get; set; }
        public RegistryValueKind RegistryType { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            this.Enabled = true;
        }

    }
}
