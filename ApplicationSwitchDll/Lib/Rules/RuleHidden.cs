using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleHidden : RuleBase
    {
        const string _RULE_NAME = "RuleHidden";

        public string TargetPath { get; set; }

        public RuleHidden(string name, string targetPath)
        {
            this.Name = name;
            this.TargetPath = targetPath;

            //  Name parameter checking.
            if (string.IsNullOrEmpty(this.Name))
            {
                return;
            }

            if (string.IsNullOrEmpty(this.TargetPath))
            {
                return;
            }

            this.Enabled = true;
        }

        public override void EnableProcess()
        {
            if (File.Exists(this.TargetPath))
            {
                File.SetAttributes(this.TargetPath, File.GetAttributes(this.TargetPath) & (~FileAttributes.Hidden));
            }
            else if (Directory.Exists(this.TargetPath))
            {
                File.SetAttributes(this.TargetPath, File.GetAttributes(this.TargetPath) & (~FileAttributes.Hidden));
            }
        }

        public override void DisableProcess()
        {
            if (File.Exists(this.TargetPath))
            {
                File.SetAttributes(this.TargetPath, File.GetAttributes(this.TargetPath) | FileAttributes.Hidden);
            }
            else if (Directory.Exists(this.TargetPath))
            {
                File.SetAttributes(this.TargetPath, File.GetAttributes(this.TargetPath) | FileAttributes.Hidden);
            }
        }
    }
}
