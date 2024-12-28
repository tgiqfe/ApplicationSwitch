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
            IEnumerable<string> enableTargets = Regex.Replace(this.Config.Target.Enable, @"\r?\n", "").
                Split(",").
                Select(x => x.Trim());
            IEnumerable<string> disableTargets = Regex.Replace(this.Config.Target.Disable, @"\r?\n", "").
                Split(",").
                Select(x => x.Trim());

            isMatch(enableTargets, Environment.MachineName);



            //this.Configs.Target;
            //ここでホスト名をもとにしてEnable/Disableを判定
            return true;

            bool isMatch(IEnumerable<string> targets, string hostName)
            {
                foreach (var target in targets)
                {
                    if (target == "*")
                    {
                        return true;
                    }
                    else if (target.Contains("*"))
                    {

                    }
                    else if (target.Contains("-"))
                    {

                    }
                    else
                    {
                        return target.Equals(hostName, StringComparison.OrdinalIgnoreCase);
                    }
                }
                return false;
            }
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