using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfig Config { get; set; }

        #region for test methods

        /// <summary>
        /// print yml.
        /// </summary>
        public void Show()
        {
            Functions.Show(this);
        }

        public string[] GetEnableTargets()
        {
            return Regex.Replace(this.Config.Target.EnableTargets ?? "", @"\r?\n", ",").
                Split(',').
                Select(x => x.Trim()).
                Where(x => !string.IsNullOrEmpty(x)).
                ToArray();
        }

        public string[] GetDisabletargets()
        {
            return Regex.Replace(this.Config.Target.DisableTargets ?? "", @"\r?\n", ",").
                Split(',').
                Select(x => x.Trim()).
                Where(x => !string.IsNullOrEmpty(x)).
                ToArray();
        }

        #endregion

        /// <summary>
        /// Varid version check from Metadata.
        /// </summary>
        /// <returns></returns>
        public bool CheckMetadataParameter()
        {
            var ret = true;
            ret &= Config.Metadata.IsParameterAll();
            ret &= Config.Metadata.IsValidVersion();
            return ret;
        }

        /// <summary>
        /// Check target enable/disable.
        /// </summary>
        /// <returns></returns>
        public bool CheckTargetParameter()
        {
            var ret = true;
            ret &= Config.Target.IsParameterAll();
            return ret;
        }

        /// <summary>
        /// Duplicate name contain check.
        /// </summary>
        /// <returns></returns>
        public bool CheckRuleParameter()
        {
            var ret = true;
            ret &= Config.Rule.IsParameterAll();
            ret &= Config.Rule.IsDuplicateName();
            return ret;
        }

        /// <summary>
        /// hostname check. enable or disable or null;
        ///     true => Enable process.
        ///     false => Disable process.
        ///     null => skip.
        /// </summary>
        /// <returns></returns>
        public bool? CheckEnableOrDisable()
        {
            return this.Config.Target.CheckEnableOrDisable();
        }

        /// <summary>
        /// Convert setting => rule object.
        /// </summary>
        /// <returns></returns>
        public RuleBase[] ConvertToRule()
        {
            return this.Config.Rule.Rules.
                Select(x => x.ConvertToRule(this.Config.Metadata.Name)).
                ToArray();
        }
    }
}
