using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleFile : RuleBase
    {
        public string TargetPath { get; set; }
        public bool RemoveEmptyParent { get; set; }
        private bool? IsFile = null;

        public RuleFile(string name, string targetPath, string removeEmptyParent)
        {
            this.Name = name;
            this.TargetPath = targetPath;



            //  Name parameter checking.
            if (string.IsNullOrEmpty(this.Name))
            {
                Logger.WriteLine("RuleFile, Name is empty.");
                return;
            }
            else
            {
                Logger.WriteLine($"RuleFile, Rule name => {this.Name}");
            }

            //  TargetPath exists checking.
            if (File.Exists(TargetPath))
            {
                IsFile = true;
                this.Enabled = true;
            }
            else if (Directory.Exists(TargetPath))
            {
                IsFile = false;
                this.Enabled = true;
            }
        }

        public override void EnableProcess()
        {
            
        }

        /// <summary>
        /// file/directory move to evacuate path.
        /// </summary>
        public override void DisableProcess()
        {
            if (!Directory.Exists(this.AppEvacuatePath))
            {
                Directory.CreateDirectory(this.AppEvacuatePath);
            }
        }
    }
}
