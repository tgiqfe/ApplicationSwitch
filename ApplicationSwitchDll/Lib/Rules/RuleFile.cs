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
        public string EvacuateFilePath { get; set; }




        public RuleFile() { }

        public override void Initialize()
        {
            this.Enabled = !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.TargetPath);
        }





        public RuleFile(string name, string appEvacuate, string targetPath, string removeEmptyParent)
        {
            this.Name = name;
            this.TargetPath = targetPath;
            this.RemoveEmptyParent = Functions.IsEnable(removeEmptyParent);

            this.TargetParent = Path.GetDirectoryName(targetPath);
            this.EvacuateFilePath = Path.Combine(this.AppEvacuatePath, Path.GetFileName(this.TargetPath));

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
            if (!Directory.Exists(this.TargetParent))
            {
                Directory.CreateDirectory(this.TargetParent);
            }

            if (File.Exists(EvacuateFilePath) && !File.Exists(this.TargetPath))
            {
                FileSystem.CopyFile(EvacuateFilePath, this.TargetPath, true);
            }
            else if (Directory.Exists(EvacuateFilePath) && !Directory.Exists(this.TargetPath))
            {
                FileSystem.CopyDirectory(EvacuateFilePath, this.TargetPath, true);
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
                FileSystem.MoveFile(this.TargetPath, EvacuateFilePath, true);

                //  remove empty parent.
                if (this.RemoveEmptyParent && Directory.GetFiles(this.TargetParent).Length > 0)
                {
                    Directory.Delete(this.TargetParent, true);
                }
            }
            else if (Directory.Exists(this.TargetPath))
            {
                FileSystem.MoveDirectory(this.TargetPath, EvacuateFilePath, true);

                //  remove empty parent.
                if (this.RemoveEmptyParent && Directory.GetFiles(this.TargetParent).Length > 0)
                {
                    Directory.Delete(this.TargetParent, true);
                }
            }
        }
    }
}
