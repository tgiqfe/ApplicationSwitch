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
                        if (rule?.Enabled ?? false)
                        {
                            rule.EnableProcess();
                        }
                    }
                    break;
                case false:
                    foreach (var ruleTemplate in this.Config.Rule.Rules)
                    {
                        var rule = ruleTemplate.ConvertToRule(evacuate);
                        if (rule?.Enabled ?? false)
                        {
                            rule.DisableProcess();
                        }
                    }
                    break;
                case null:
                    Logger.WriteLine($"{this.Config.Metadata.Name} is not applicable.");
                    break;
            }
        }
    }
}