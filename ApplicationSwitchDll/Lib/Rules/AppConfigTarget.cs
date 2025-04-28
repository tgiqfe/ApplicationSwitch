using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Rules
{
    internal class AppConfigTarget
    {
        [YamlMember(Alias = "Enable")]
        public string EnableTargetts { get; set; }

        [YamlMember(Alias = "Disable")]
        public string DisableTargetts { get; set; }



    }
}
