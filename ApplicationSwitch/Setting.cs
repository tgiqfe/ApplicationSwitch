﻿using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Yml;
using YamlDotNet.Serialization;

namespace ApplicationSwitch
{
    internal class Setting
    {
        public string LogDirectory { get; set; }
        public string EvacuateDirectory { get; set; }
        public bool EvacuateHidden { get; set; }
        public string ConfigDirectory { get; set; }
    }
}
