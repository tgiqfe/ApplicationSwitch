using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    internal class AppConfigRoot
    {
        [YamlMember(Alias = "App")]
        public Dictionary<string, AppConfig> Configs { get; set; }
    }
}
