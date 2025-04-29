using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfig Config { get; set; }

        private string AppEvacuatePath
        {
            get { return Path.Combine(Item.EvacuateDirectory, Config.Metadata.Name); }
        }

        public void Show()
        {
            Functions.Show(this);
        }

        /// <summary>
        /// Varid version check from Metadata.
        /// </summary>
        /// <returns></returns>
        public bool CheckMetadata()
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
        public bool CheckTarget()
        {
            var ret = true;
            ret &= Config.Target.IsParameterAll();
            ret &= Config.Target.CheckEnableOrDisable() ?? true;
            return ret;
        }

        /// <summary>
        /// Duplicate name contain check.
        /// </summary>
        /// <returns></returns>
        public bool CheckRule()
        {
            var ret = true;
            ret &= Config.Rule.IsParameterAll();
            ret &= Config.Rule.IsDuplicateName();
            return ret;
        }
    }
}
