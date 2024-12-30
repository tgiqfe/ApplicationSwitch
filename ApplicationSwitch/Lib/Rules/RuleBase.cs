using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleBase
    {
        public string Name { get; set; }

        public bool Enabled { get; protected set; }

        public string AppEvacuate { get; set; }

        protected string AppEvacuatePath
        {
            get
            {
                return Path.Combine(this.AppEvacuate, this.Name);
            }
        }

        public virtual void Initialize()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                Logger.WriteLine("RuleFile, Name is empty.");
                return;
            }
            Logger.WriteLine($"RuleFile, Rule name => {this.Name}");
        }

        public virtual void EnableProcess() { }

        public virtual void DisableProcess() { }
    }
}
