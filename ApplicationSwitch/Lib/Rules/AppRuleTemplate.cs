using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Rules
{
    internal class AppRuleTemplate
    {
        public string Action { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }
        public string Execute { get; set; }
        public string Arguments { get; set; }

        private readonly static string[] candidate_File = new string[] { "File", "fil", "filemove" };
        private readonly static string[] candidate_Registry = new string[] { "Registry", "reg", "RegistryKey", "RegistryValue", "RegistryParam" };

        public RuleBase ConvertToRule(string evacuate)
        {
            return this.Action switch
            {
                string s when candidate_File.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleFile(
                    this.Name,
                    evacuate,
                    this.Target),
                string s when candidate_Registry.Any(x => x.Equals(x, StringComparison.OrdinalIgnoreCase)) => new RuleRegistry(
                    this.Name,
                    evacuate,
                    this.RegistryKey,
                    this.RegistryParam),
                _ => null
            };

        }
    }
}
