namespace ApplicationSwitch.Lib.Rules
{
    internal class AppRuleTemplate
    {
        public string Action { get; set; }
        public string Name { get; set; }
        public string TargetPath { get; set; }
        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }
        public string EnableCommand { get; set; }
        public string DisableCommand { get; set; }
        public string EnableScript { get; set; }
        public string DisableScript { get; set; }

        private readonly static string[] candidate_File = new string[] { "File", "fil", "filemove" };
        private readonly static string[] candidate_Registry = new string[] { "Registry", "reg", "RegistryKey", "RegistryValue", "RegistryParam" };
        private readonly static string[] candidate_Command = new string[] { "Command", "cmd" };

        public RuleBase ConvertToRule(string evacuate)
        {
            return this.Action switch
            {
                string s when candidate_File.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleFile(
                    this.Name,
                    evacuate,
                    this.TargetPath),
                string s when candidate_Registry.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleRegistry(
                    this.Name,
                    evacuate,
                    this.RegistryKey,
                    this.RegistryParam),
                string s when candidate_Command.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => new RuleCommand(
                    this.Name,
                    evacuate,
                    this.EnableCommand,
                    this.DisableCommand,
                    this.EnableScript,
                    this.DisableScript),
                _ => null
            };
        }
    }
}
