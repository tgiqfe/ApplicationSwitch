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

        [YamlIgnore]
        public string AppEvacuatePath
        {
            get { return Path.Combine(Item.EvacuateDirectory, Config.Metadata.Name); }
        }

        /// <summary>
        /// Varid version check from Metadata.
        /// </summary>
        /// <returns></returns>
        public bool CheckMetadata()
        {
            return Config.Metadata.IsValidVersion();
        }

        public bool CheckTarget()
        {
            var ret = Config.Target.CheckEnableOrDisable();
            return ret ?? true;
        }

        /// <summary>
        /// Duplicate name contain check.
        /// </summary>
        /// <returns></returns>
        public bool CheckRule()
        {
            return Config.Rule.IsDuplicateName();
        }
    }
}
