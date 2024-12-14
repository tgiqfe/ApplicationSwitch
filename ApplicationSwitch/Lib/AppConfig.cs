using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib
{
    internal class AppConfig
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HostNames { get; set; }

        public List<RuleBase> Rules { get; set; }
    }
}
