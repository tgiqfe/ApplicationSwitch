using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppConfigTarget
    {
        #region Parameter

        /// <summary>
        /// Enable targets (enable or disable mandatory)
        /// </summary>
        [YamlMember(Alias = "Enable")]
        public string EnableTargets { get; set; }

        /// <summary>
        /// Disable targets (enable or disable mandatory)
        /// </summary>
        [YamlMember(Alias = "Disable")]
        public string DisableTargets { get; set; }

        public string PrimaryTarget { get; set; }

        #endregion

        /// <summary>
        /// Parameter check
        /// </summary>
        /// <returns></returns>
        public bool IsParameterAll()
        {
            return !string.IsNullOrEmpty(this.EnableTargets) || !string.IsNullOrEmpty(this.DisableTargets);
        }

        public bool? CheckEnableOrDisable()
        {
            if (Functions.IsDisable(this.PrimaryTarget))
            {
                var disRet = __GetTargets(DisableTargets).
                    Any(x => x.IsMatch(Item.Hostname, Item.Hostname_baseName, Item.Hostname_number, Item.Hostname_length));
                if (disRet) return false;

                var enRet = __GetTargets(EnableTargets).
                    Any(x => x.IsMatch(Item.Hostname, Item.Hostname_baseName, Item.Hostname_number, Item.Hostname_length));
                if (enRet) return true;
            }
            else
            {
                var enRet = __GetTargets(EnableTargets).
                    Any(x => x.IsMatch(Item.Hostname, Item.Hostname_baseName, Item.Hostname_number, Item.Hostname_length));
                if (enRet) return true;

                var disRet = __GetTargets(DisableTargets).
                    Any(x => x.IsMatch(Item.Hostname, Item.Hostname_baseName, Item.Hostname_number, Item.Hostname_length));
                if (disRet) return false;
            }

            return null;

            IEnumerable<ConfigTarget> __GetTargets(string targetText)
            {
                return Regex.Replace(targetText ?? "", @"\r?\n", ",").
                    Split(",").
                    Select(x => x.Trim()).
                    Where(x => !string.IsNullOrEmpty(x)).
                    Select(x => new ConfigTarget(x));
            }
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
