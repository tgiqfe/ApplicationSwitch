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
                Logger.WriteLine("RuleFile, Name is empty.");
                return;
            }
            Logger.WriteLine($"RuleFile, Rule name => {this.Name}");
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
                Logger.WriteLine($"RuleFile, File restore => {EvacuateFilePath} to {this.TargetPath}", 4);
                FileSystem.CopyFile(EvacuateFilePath, this.TargetPath, true);
            }
            else if (Directory.Exists(EvacuateFilePath) && !Directory.Exists(this.TargetPath))
            {
                Logger.WriteLine($"RuleFile, Directory restore => {EvacuateFilePath} to {this.TargetPath}", 4);
                FileSystem.CopyDirectory(EvacuateFilePath, this.TargetPath, true);
            }
            else
            {
                //  not applicable.
                Logger.WriteLine("RuleFile, Restore not applicable.", 4);
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
                Logger.WriteLine($"RuleFile, File evacuate => {this.TargetPath} to {EvacuateFilePath}", 4);
                FileSystem.MoveFile(this.TargetPath, EvacuateFilePath, true);

                //  remove empty parent.
                if (this.RemoveEmptyParent && Directory.GetFiles(this.TargetParent).Length > 0)
                {
                    Logger.WriteLine($"RuleFile, Remove empty parent => {this.TargetParent}", 4);
                    Directory.Delete(this.TargetParent, true);
                }
            }
            else if (Directory.Exists(this.TargetPath))
            {
                Logger.WriteLine($"RuleFile, Directory evacuate => {this.TargetPath} to {EvacuateFilePath}", 4);
                FileSystem.MoveDirectory(this.TargetPath, EvacuateFilePath, true);

                //  remove empty parent.
                if (this.RemoveEmptyParent && Directory.GetFiles(this.TargetParent).Length > 0)
                {
                    Logger.WriteLine($"RuleFile, Remove empty parent => {this.TargetParent}", 4);
                    Directory.Delete(this.TargetParent, true);
                }
            }
            else
            {
                //  not applicable.
                Logger.WriteLine("RuleFile, Evacuate not applicable.", 4);
            }
        }
    }
}
