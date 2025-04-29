using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    public class RuleBase
    {
        protected string RuleTypeName { get { return this.GetType().Name; } }

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
                return "";
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
