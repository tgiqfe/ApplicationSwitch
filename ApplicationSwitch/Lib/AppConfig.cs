using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSwitch.Lib.Manifest;

namespace ApplicationSwitch.Lib
{
    internal class AppConfig
    {
        public AppConfigMetadata Metadata { get; set; }
        public AppConfigTarget Target { get; set; }
        public AppConfigRule Rule { get; set; }
    }
}
