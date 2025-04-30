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

        private string EvacuateFilePath
        {
            get { return Path.Combine(this.EvacuateParentPath, Path.GetFileName(this.TargetPath)); }
        }

        public override void Initialize()
        {
            this.Enabled =
                !string.IsNullOrEmpty(this.Name) &&
                !string.IsNullOrEmpty(this.TargetPath);
        }

        public override void EnableProcess()
        {
            string parent = Path.GetDirectoryName(this.TargetPath);
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }

            if (File.Exists(EvacuateFilePath) && !File.Exists(this.TargetPath))
            {
                FileSystem.CopyFile(EvacuateFilePath, this.TargetPath, true);
            }
            else if (Directory.Exists(EvacuateFilePath) && !Directory.Exists(this.TargetPath))
            {
                FileSystem.CopyDirectory(EvacuateFilePath, this.TargetPath, true);
            }

            EndProcess();
        }

        /// <summary>
        /// file/directory move to evacuate path.
        /// </summary>
        public override void DisableProcess()
        {
            if (!Directory.Exists(this.EvacuateParentPath))
            {
                Directory.CreateDirectory(this.EvacuateParentPath);
            }

            if (File.Exists(this.TargetPath))
            {
                FileSystem.MoveFile(this.TargetPath, EvacuateFilePath, true);

                //  remove empty parent.
                if (this.RemoveEmptyParent)
                {
                    string targetParent = Path.GetDirectoryName(this.TargetPath);
                    if (Directory.GetFiles(targetParent).Length == 0)
                    {
                        //Directory.Delete(targetParent, true);
                    }
                }
            }
            else if (Directory.Exists(this.TargetPath))
            {
                FileSystem.MoveDirectory(this.TargetPath, EvacuateFilePath, true);

                //  remove empty parent.
                if (this.RemoveEmptyParent)
                {
                    string targetParent = Path.GetDirectoryName(this.TargetPath);
                    if (Directory.GetFiles(targetParent).Length == 0)
                    {
                        //Directory.Delete(targetParent, true);
                    }
                }
            }

            EndProcess();
        }
    }
}
