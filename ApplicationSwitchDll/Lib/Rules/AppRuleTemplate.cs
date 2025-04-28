using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class AppRuleTemplate
    {
        public string Action { get; set; }
        public string Name { get; set; }
        public string TargetPath { get; set; }
        public string RemoveEmptyParent { get; set; }
        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }
        public string EnableCommand { get; set; }
        public string DisableCommand { get; set; }
        public string EnableScript { get; set; }
        public string DisableScript { get; set; }

        private readonly static string[] candidate_File = new string[] { "File", "fil", "filemove" };
        private readonly static string[] candidate_Registry = new string[] { "Registry", "reg", "RegistryKey", "RegistryValue", "RegistryParam" };
        private readonly static string[] candidate_Command = new string[] { "Command", "cmd" };
        private readonly static string[] candidate_Hidden = new string[] { "Hidden", "Hide", "Hiden" };

        public RuleBase ConvertToRUle()
        {
            return this.Action switch
            {
                string s when candidate_File.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleFile(
                    this.Name,
                    this.TargetPath,
                    this.RemoveEmptyParent),
                _ => null,
            };
        }
    }
}
