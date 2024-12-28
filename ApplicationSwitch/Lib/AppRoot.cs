using ApplicationSwitch.Lib.Yml;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    internal class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfig Config { get; set; }

        public void ProcessFromRule()
        {
            var endis = CheckEnDis();
            if (endis == null) return;
            RuleProcess((bool)endis);
        }

        /// <summary>
        /// Check Enable/Disable by HostName.
        /// </summary>
        /// <returns></returns>
        private bool? CheckEnDis()
        {
            //this.Configs.Target;
            //ここでホスト名をもとにしてEnable/Disableを判定
            return true;
        }

        /// <summary>
        /// Rule process from rule.
        /// </summary>
        /// <param name="endis"></param>
        private void RuleProcess(bool endis)
        {
            foreach (var ruleTemplate in this.Config.Rule.Rules)
            {
                var rule = ruleTemplate.ConvertToRule(this.Config.Metadata.Evacuate);
                if (endis)
                {
                    rule.EnableProcess();
                }
                else
                {
                    rule.DisableProcess();
                }
            }
        }
    }
}