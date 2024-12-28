using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleTemplate
    {
        public string Action { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }
        public string RegistryValue { get; set; }  
        public string RegistryType { get; set; }
        public string Access { get; set; }
        public string Inherited { get; set; }
        public string Inheritance { get; set; }
        public string Owner { get; set; }
        public string Execute { get; set; }
        public string Arguments { get; set; }
    }
}
