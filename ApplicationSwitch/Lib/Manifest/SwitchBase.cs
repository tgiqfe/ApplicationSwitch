using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Manifest
{
    internal class SwitchBase
    {
        public string Name { get; set; }

        public virtual void ToHidden() { }

        public virtual void ToVisible() { }
    }
}
