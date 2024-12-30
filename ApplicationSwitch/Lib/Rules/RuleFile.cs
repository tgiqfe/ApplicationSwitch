using Microsoft.VisualBasic.FileIO;
using System.Reflection.Metadata.Ecma335;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleFile : RuleBase
    {
        public string TargetPath { get; set; }

        public RuleFile(string name, string evacuate, string targetPath)
        {
            this.Name = name;
            this.AppEvacuate = evacuate;
            this.TargetPath = targetPath;

            if (string.IsNullOrEmpty(this.Name))
            {
                Logger.WriteLine("RuleFile, Name is empty.");
                return;
            }
            Logger.WriteLine($"RuleFile, Rule name => {this.Name}");
            this.Enabled = true;
        }

        /// <summary>
        /// file move process for Enable target.
        /// </summary>
        public override void EnableProcess()
        {
            var parent = Path.GetDirectoryName(this.TargetPath);
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }

            var source = Path.Combine(this.AppEvacuatePath, Path.GetFileName(this.TargetPath));
            if (File.Exists(source) && !File.Exists(this.TargetPath))
            {
                //  File evacuate.
                Logger.WriteLine($"RuleFile, File restore => {source} to {this.TargetPath}", 4);
                FileSystem.CopyFile(source, this.TargetPath, true);
            }
            else if (Directory.Exists(source) && !Directory.Exists(this.TargetPath))
            {
                //  Directory evacuate.
                Logger.WriteLine($"RuleFile, Directory restore => {source} to {this.TargetPath}", 4);
                FileSystem.CopyDirectory(source, this.TargetPath, true);
            }
            else
            {
                //  not applicable.
                Logger.WriteLine("RuleFile, Restore not applicable.", 4);
            }
        }

        /// <summary>
        /// file move process for Disable target.
        /// </summary>
        public override void DisableProcess()
        {
            if (!Directory.Exists(this.AppEvacuatePath))
            {
                Directory.CreateDirectory(this.AppEvacuatePath);
            }

            var destination = Path.Combine(this.AppEvacuatePath, Path.GetFileName(this.TargetPath));
            if (File.Exists(this.TargetPath))
            {
                //  File evacuate.
                Logger.WriteLine($"RuleFile, File evacuate => {this.TargetPath} to {destination}", 4);
                FileSystem.MoveFile(this.TargetPath, destination, true);
            }
            else if (Directory.Exists(this.TargetPath))
            {
                //  Directory evacuate.
                Logger.WriteLine($"RuleFile, Directory evacuate => {this.TargetPath} to {destination}", 4);
                FileSystem.MoveDirectory(this.TargetPath, destination, true);
            }
            else
            {
                //  not applicable.
                Logger.WriteLine("RuleFile, Evacuate not applicable.", 4);
            }
        }
    }
}
