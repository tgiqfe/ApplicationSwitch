﻿using System;
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
        public string Target { get; set; }
        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }
        public string RegistryValue { get; set; }
        public string RegistryType { get; set; }
        public string Execute { get; set; }
        public string Arguments { get; set; }

        private readonly static string[] candidate_File = new string[] { "File", "fil", "filemove" };

        public RuleBase ConvertToRule(string evacuate)
        {
            return this.Action switch
            {
                string s when candidate_File.Any(x => s.Equals(StringComparison.OrdinalIgnoreCase)) => new RuleFile()
                {
                    Name = this.Name,
                    Target = this.Target,
                },
                _ => null
            };

        }
    }
}