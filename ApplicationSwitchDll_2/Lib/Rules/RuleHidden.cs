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
                Logger.WriteLine($"{_RULE_NAME}, Name is empty.");
                return;
            }
            Logger.WriteLine($"{_RULE_NAME}, Rule name => {this.Name}");

            if (string.IsNullOrEmpty(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, Target path is empty.");
                return;
            }
            Logger.WriteLine($"{_RULE_NAME}, Target path => {this.TargetPath}");

            this.Enabled = true;
        }

        public override void EnableProcess()
        {
            if (File.Exists(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, File hidden -> visible => {this.TargetPath}", 4);
                File.SetAttributes(this.TargetPath, File.GetAttributes(this.TargetPath) & (~FileAttributes.Hidden));
            }
            else if (Directory.Exists(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, Directory hidden -> visible => {this.TargetPath}", 4);
                File.SetAttributes(this.TargetPath, File.GetAttributes(this.TargetPath) & (~FileAttributes.Hidden));
            }
        }

        public override void DisableProcess()
        {
            if (File.Exists(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, File visible -> hidden => {this.TargetPath}", 4);
                File.SetAttributes(this.TargetPath, File.GetAttributes(this.TargetPath) | FileAttributes.Hidden);
            }
            else if (Directory.Exists(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, Directory visible -> hidden => {this.TargetPath}", 4);
                File.SetAttributes(this.TargetPath, File.GetAttributes(this.TargetPath) | FileAttributes.Hidden);
            }
        }
    }
}
