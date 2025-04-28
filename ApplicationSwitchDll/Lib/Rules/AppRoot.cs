using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Rules
{
    internal class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfigMetadata Config { get; set; }


    }
}
