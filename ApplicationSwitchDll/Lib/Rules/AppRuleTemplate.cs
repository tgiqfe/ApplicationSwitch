using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    [SupportedOSPlatform("windows")]
    public class AppRuleTemplate
    {
        public string Action { get; set; }
        public string Name { get; set; }
        public string TargetPath { get; set; }

        /// <summary>
        /// for RuleFile
        /// When disabled, delete the parent folder.
        /// </summary>
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

        public RuleBase ConvertToRule()
        {
            return this.Action switch
            {
                string s when candidate_File.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleFile(
                    this.Name,
                    this.TargetPath,
                    this.RemoveEmptyParent),
                string s when candidate_Registry.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleRegistry(
                    this.Name,
                    this.RegistryKey,
                    this.RegistryParam),
                _ => null,
            };
        }
    }
}
