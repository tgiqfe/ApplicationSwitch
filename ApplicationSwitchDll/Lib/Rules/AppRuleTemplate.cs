﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppRuleTemplate
    {
        /// <summary>
        /// Rule action type (mandatory)
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Rule name (mandatory)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// for RuleFile, for Hidden
        /// process target file/directory.
        /// </summary>
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

        public RuleBase ConvertToRule(string parentNamae)
        {
            RuleBase rule = this.Action switch
            {
                string s when candidate_File.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleFile()
                {
                    Parent = parentNamae,
                    Name = this.Name,
                    TargetPath = Functions.ExpandEnvironmentText(this.TargetPath),
                    RemoveEmptyParent = Functions.IsEnable(this.RemoveEmptyParent),
                },
                string s when candidate_Registry.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleRegistry()
                {
                    Parent = parentNamae,
                    Name = this.Name,
                    RegistryKey = this.RegistryKey,
                    RegistryParam = this.RegistryParam,
                },
                string s when candidate_Command.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleCommand()
                {
                    Parent = parentNamae,
                    Name = this.Name,
                    EnableCommand = this.EnableCommand,
                    DisableCommand = this.DisableCommand,
                    EnableScript = Functions.ExpandEnvironmentText(this.EnableScript),
                    DisableScript = Functions.ExpandEnvironmentText(this.DisableScript),
                },
                string s when candidate_Hidden.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleHidden()
                {
                    Parent = parentNamae,
                    Name = this.Name,
                    TargetPath = Functions.ExpandEnvironmentText(this.TargetPath),
                },
                _ => null,
            };
            rule.Initialize();

            return rule;
        }
    }
}
