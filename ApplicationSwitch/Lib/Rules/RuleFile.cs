using Microsoft.VisualBasic.FileIO;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleFile : RuleBase
    {
        public string TargetPath { get; set; }

        public override void Initialize()
        {
            base.Initialize();
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
                FileSystem.CopyFile(source, this.TargetPath, true);
            }
            else if (Directory.Exists(source) && !Directory.Exists(this.TargetPath))
            {
                //  Directory evacuate.
                FileSystem.CopyDirectory(source, this.TargetPath, true);
            }
            else
            {
                //  move not eligible,
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
                FileSystem.MoveFile(this.TargetPath, destination, true);
            }
            else if (Directory.Exists(this.TargetPath))
            {
                //  Directory evacuate.
                FileSystem.MoveDirectory(this.TargetPath, destination, true);
            }
            else
            {
                //  Target file or directory not found.
            }
        }
    }
}
