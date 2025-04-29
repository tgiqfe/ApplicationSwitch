using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppConfigTarget
    {
        [YamlMember(Alias = "Enable")]
        public string EnableTargets { get; set; }

        [YamlMember(Alias = "Disable")]
        public string DisableTargets { get; set; }

        public string PrimaryTarget { get; set; }

        private IEnumerable<ConfigTarget> _enableConfigTargets = null;
        private IEnumerable<ConfigTarget> _disableConfigTargets = null;
        private IEnumerable<ConfigTarget> EnableConfigTargets
        {
            get
            {
                _enableConfigTargets ??= Regex.Replace(this.EnableTargets ?? "", @"\r?\n", ",").
                    Split(",").
                    Select(x => x.Trim()).
                    Where(x => !string.IsNullOrEmpty(x)).
                    Select(x => new ConfigTarget(x));
                return _enableConfigTargets;
            }
        }
        private IEnumerable<ConfigTarget> DisableConfigTargets
        {
            get
            {
                _disableConfigTargets ??= Regex.Replace(this.DisableTargets ?? "", @"\r?\n", ",").
                    Split(",").
                    Select(x => x.Trim()).
                    Where(x => !string.IsNullOrEmpty(x)).
                    Select(x => new ConfigTarget(x));
                return _disableConfigTargets;
            }
        }

        public bool? CheckEnableOrDisable(string hostName)
        {
            string numText = Regex.Match(hostName, @"\d*$").Value;
            int? num = int.TryParse(numText, out int n) ? n : null;
            string baseName = hostName.Substring(0, hostName.Length - numText.Length);
            int length = hostName.Length;

            if (Functions.IsDisable(this.PrimaryTarget))
            {
                var ret = DisableConfigTargets.Any(x => x.IsMatch(hostName, baseName, num, length));
                if (ret) return false;

                ret = EnableConfigTargets.Any(x => x.IsMatch(hostName, baseName, num, length));
                if (ret) return true;
            }
            else
            {
                var ret = EnableConfigTargets.Any(x => x.IsMatch(hostName, baseName, num, length));
                if (ret) return true;

                ret = DisableConfigTargets.Any(x => x.IsMatch(hostName, baseName, num, length));
                if (ret) return false;
            }

            return null;
        }

        #region ConfgTarget class

        private enum ConfigTargetType
        {
            None,
            AnyMatch,
            WildcardMatch,
            RangeMatch,
            FullMatch,
        }

        private class ConfigTarget
        {
            public string Name { get; private set; }
            public int Length { get; private set; }
            public ConfigTargetType Type { get; private set; }
            public Regex WildcardPattern { get; private set; }
            public string RangeBaseName { get; private set; }
            public string RangePreNumText { get; private set; }
            public string RangeSufNumText { get; private set; }
            public int RangePreNum { get; private set; }
            public int RangeSufNum { get; private set; }

            public ConfigTarget(string name)
            {
                this.Name = name;
                if (name == "*")
                {
                    this.Length = -1;
                    this.Type = ConfigTargetType.AnyMatch;
                }
                else if (name.Contains("*"))
                {
                    this.Length = -1;
                    this.Type = ConfigTargetType.WildcardMatch;
                    string tempRegexText = name.Replace("*", ".*");
                    if (!tempRegexText.StartsWith("^")) tempRegexText = "^" + tempRegexText;
                    if (!tempRegexText.EndsWith("$")) tempRegexText = tempRegexText + "$";
                    this.WildcardPattern = new Regex(tempRegexText, RegexOptions.IgnoreCase);
                }
                else if (name.Contains("~"))
                {
                    this.Type = ConfigTargetType.RangeMatch;
                    this.RangeSufNumText = name.Substring(name.IndexOf('~') + 1);
                    int digit = RangeSufNumText.Length;
                    this.RangeBaseName = name.Substring(0, name.IndexOf('~') - digit);
                    this.RangePreNumText = name.Substring(name.IndexOf('~') - digit, digit);
                    this.RangePreNum = int.TryParse(RangePreNumText, out int preNum) ? preNum : -1;
                    this.RangeSufNum = int.TryParse(RangeSufNumText, out int sufNum) ? sufNum : -1;
                    this.Length = RangeBaseName.Length + digit;
                }
                else
                {
                    this.Length = name.Length;
                    this.Type = ConfigTargetType.FullMatch;
                }
            }

            public bool IsMatch(string hostName, string baseName, int? num, int length)
            {
                switch (this.Type)
                {
                    case ConfigTargetType.AnyMatch:
                        return true;
                    case ConfigTargetType.WildcardMatch:
                        return this.WildcardPattern.IsMatch(hostName);
                    case ConfigTargetType.RangeMatch:
                        if (num == null) return false;
                        int n = (int)num;
                        return this.Length == length &&
                            this.RangeBaseName.Equals(baseName, StringComparison.OrdinalIgnoreCase) &&
                            this.RangePreNum <= n &&
                            n <= this.RangeSufNum;
                    case ConfigTargetType.FullMatch:
                        return this.Length == length &&
                            this.Name.Equals(hostName, StringComparison.OrdinalIgnoreCase);
                    default:
                        return false;
                }
            }
        }

        #endregion
    }
}
