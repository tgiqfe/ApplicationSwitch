using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    /// <summary>
    /// Application configuration root.
    /// </summary>
    internal class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfig Config { get; set; }

        public void ProcessRules(string evacuateDirectory)
        {
            string evacuate = Path.Combine(evacuateDirectory, this.Config.Metadata.Name);
            var endis = this.Config.Target.CheckEnDis(Environment.MachineName);
            switch (endis)
            {
                case true:
                    foreach (var ruleTemplate in this.Config.Rule.Rules)
                    {
                        var rule = ruleTemplate.ConvertToRule(evacuate);
                        rule.EnableProcess();
                    }
                    break;
                case false:
                    foreach (var ruleTemplate in this.Config.Rule.Rules)
                    {
                        var rule = ruleTemplate.ConvertToRule(evacuate);
                        rule.DisableProcess();
                    }
                    break;
                case null:
                    break;
            }
        }
    }
}