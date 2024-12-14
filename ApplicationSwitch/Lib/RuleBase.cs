using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib
{
    internal class RuleBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Index { get; set; }

        public virtual void ToHidden() { }

        public virtual void ToVisible() { }

    }
}
