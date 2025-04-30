using System.IO;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleHidden : RuleBase
    {
        public string TargetPath { get; set; }

        public override void Initialize()
        {
            this.Enabled = !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.TargetPath);
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

            EndProcess(isEnableProcess: true);
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

            EndProcess(isEnableProcess: false);
        }
    }
}
