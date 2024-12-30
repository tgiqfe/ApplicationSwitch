using ApplicationSwitch.Lib.Yml;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    internal class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfig Config { get; set; }

        public void ProcessFromRule()
        {
            var endis = this.Config.Target.CheckEnDis(Environment.MachineName);
            Console.WriteLine(endis);
            /*
            switch (endis)
            {
                case true:
                    foreach (var ruleTemplate in this.Config.Rule.Rules)
                    {
                        var rule = ruleTemplate.ConvertToRule(this.Config.Metadata.Evacuate);
                        rule.EnableProcess();
                    }
                    break;
                case false:
                    foreach (var ruleTemplate in this.Config.Rule.Rules)
                    {
                        var rule = ruleTemplate.ConvertToRule(this.Config.Metadata.Evacuate);
                        rule.DisableProcess();
                    }
                    break;
                case null:
                    break;
            }
            */
        }

    }
}