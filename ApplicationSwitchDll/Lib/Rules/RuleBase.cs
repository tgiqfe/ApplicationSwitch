using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleBase
    {
        /// <summary>
        /// Rule name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Rule enable / disable
        /// </summary>
        public bool Enabled { get; protected set; }

        protected string AppEvacuatePath
        {
            get
            {
                return Path.Combine(Item.EvacuateDirectory, this.Name);
            }
        }

        /// <summary>
        /// Application Evacuate -> Source
        /// </summary>
        public virtual void EnableProcess() { }

        /// <summary>
        /// /// Application Source -> Evacuate
        /// </summary>
        public virtual void DisableProcess() { }
    }
}
