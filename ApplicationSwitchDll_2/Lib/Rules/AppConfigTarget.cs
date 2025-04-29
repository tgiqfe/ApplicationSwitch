using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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



    }
}
