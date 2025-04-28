using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleFile : RuleBase
    {
        const string _RULE_NAME = "RuleFile";

        public string TargetPath { get; set; }
        public bool RemoveEmptyParent { get; set; }

        private string TargetParent { get; set; }
        private string EvacuateFilePath { get; set; }

        public RuleFile(string name, string targetPath, string removeEmptyParent)
        {
            this.Name = name;
            this.TargetPath = targetPath;
            this.RemoveEmptyParent = Functions.IsEnable(removeEmptyParent);

            this.TargetParent = Path.GetDirectoryName(targetPath);
            this.EvacuateFilePath = Path.Combine(this.AppEvacuatePath, Path.GetFileName(this.TargetPath));

            //  Name parameter checking.
            if (string.IsNullOrEmpty(this.Name))
            {
                Logger.WriteLine($"{_RULE_NAME}, Name is empty.");
                return;
            }
            Logger.WriteLine($"{_RULE_NAME}, Rule name => {this.Name}");

            if(string.IsNullOrEmpty(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, Target path is empty.");
                return;
            }
            Logger.WriteLine($"{_RULE_NAME}, Target path => {this.TargetPath}");

            this.Enabled = true;
        }

        public override void EnableProcess()
        {
            if (!Directory.Exists(this.TargetParent))
            {
                Directory.CreateDirectory(this.TargetParent);
            }

            if (File.Exists(EvacuateFilePath) && !File.Exists(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, File restore => {EvacuateFilePath} to {this.TargetPath}", 4);
                FileSystem.CopyFile(EvacuateFilePath, this.TargetPath, true);
            }
            else if (Directory.Exists(EvacuateFilePath) && !Directory.Exists(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, Directory restore => {EvacuateFilePath} to {this.TargetPath}", 4);
                FileSystem.CopyDirectory(EvacuateFilePath, this.TargetPath, true);
            }
            else
            {
                //  not applicable.
                Logger.WriteLine($"{_RULE_NAME}, Restore not applicable.", 4);
            }
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

            //  evacuate.
            if (File.Exists(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, File evacuate => {this.TargetPath} to {EvacuateFilePath}", 4);
                FileSystem.MoveFile(this.TargetPath, EvacuateFilePath, true);

                //  remove empty parent.
                if (this.RemoveEmptyParent && Directory.GetFiles(this.TargetParent).Length > 0)
                {
                    Logger.WriteLine($"{_RULE_NAME}, Remove empty parent => {this.TargetParent}", 4);
                    Directory.Delete(this.TargetParent, true);
                }
            }
            else if (Directory.Exists(this.TargetPath))
            {
                Logger.WriteLine($"{_RULE_NAME}, Directory evacuate => {this.TargetPath} to {EvacuateFilePath}", 4);
                FileSystem.MoveDirectory(this.TargetPath, EvacuateFilePath, true);

                //  remove empty parent.
                if (this.RemoveEmptyParent && Directory.GetFiles(this.TargetParent).Length > 0)
                {
                    Logger.WriteLine($"{_RULE_NAME}, Remove empty parent => {this.TargetParent}", 4);
                    Directory.Delete(this.TargetParent, true);
                }
            }
            else
            {
                //  not applicable.
                Logger.WriteLine("{_RULE_NAME}, Evacuate not applicable.", 4);
            }
        }
    }
}
