using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppConfig
    {
        public AppConfigMetadata Metadata { get; set; }
        public AppConfigTarget Target { get; set; }
        public AppConfigRule Rule { get; set; }
    }
}
