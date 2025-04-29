using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    public class RuleBase
    {
        public string Parent { get; set; }

        public string AppEvacuatePath
        {
            get { return Path.Combine(Item.EvacuateDirectory, this.Parent, this.Name); }
        }

        /// <summary>
        /// Rule name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Rule enable / disable
        /// </summary>
        public bool Enabled { get; protected set; }

        /// <summary>
        /// Init parameter setting.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Application Evacuate -> Source
        /// </summary>
        public virtual void EnableProcess() { }

        /// <summary>
        /// /// Application Source -> Evacuate
        /// </summary>
        public virtual void DisableProcess() { }

        /// <summary>
        /// end process. for Enable or Disable process
        /// </summary>
        protected void EndProcess()
        {
            if (Item.HiddenEvacuateDirectory)
            {
                var attr = File.GetAttributes(Item.EvacuateDirectory);
                if ((attr & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    File.SetAttributes(Item.EvacuateDirectory, attr | FileAttributes.Hidden);
                }
            }
        }
    }
}
