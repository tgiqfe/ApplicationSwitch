using Microsoft.VisualBasic.FileIO;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleFile : RuleBase
    {
        public string TargetPath { get; set; }
        public bool RemoveEmptyParent { get; set; }

        private string EvacuateFilePath
        {
            get { return Path.Combine(this.EvacuateRulePath, Path.GetFileName(this.TargetPath)); }
        }

        public override void Initialize()
        {
            this.Enabled =
                !string.IsNullOrEmpty(this.Name) &&
                !string.IsNullOrEmpty(this.TargetPath);
        }

        /// <summary>
        /// file/directory restore to source path.
        /// </summary>
        public override void EnableProcess()
        {
            string parent = Path.GetDirectoryName(this.TargetPath);

            if (File.Exists(EvacuateFilePath) && !File.Exists(this.TargetPath))
            {
                if (!Directory.Exists(parent))
                {
                    Directory.CreateDirectory(parent);
                }
                FileSystem.CopyFile(EvacuateFilePath, this.TargetPath, true);
            }
            else if (Directory.Exists(EvacuateFilePath) && !Directory.Exists(this.TargetPath))
            {
                if (!Directory.Exists(parent))
                {
                    Directory.CreateDirectory(parent);
                }
                FileSystem.CopyDirectory(EvacuateFilePath, this.TargetPath, true);
            }

            EndProcess(isEnableProcess: true);
        }

        /// <summary>
        /// file/directory move to evacuate path.
        /// </summary>
        public override void DisableProcess()
        {
            if (!Directory.Exists(this.EvacuateRulePath))
            {
                Directory.CreateDirectory(this.EvacuateRulePath);
            }

            if (File.Exists(this.TargetPath))
            {
                FileSystem.MoveFile(this.TargetPath, EvacuateFilePath, true);

                if (this.RemoveEmptyParent) __RemoveEmptyParentDirectory(this.TargetPath);

            }
            else if (Directory.Exists(this.TargetPath))
            {
                FileSystem.MoveDirectory(this.TargetPath, EvacuateFilePath, true);

                if (this.RemoveEmptyParent) __RemoveEmptyParentDirectory(this.TargetPath);
            }

            EndProcess(isEnableProcess: false);

            void __RemoveEmptyParentDirectory(string targetPath)
            {
                string parent = Path.GetDirectoryName(targetPath);
                if (Directory.GetFiles(parent).Length == 0 && Directory.GetDirectories(parent).Length == 0)
                {
                    Directory.Delete(parent);
                }
            }
        }
    }
}
