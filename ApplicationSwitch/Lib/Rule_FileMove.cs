using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace ApplicationSwitch.Lib
{
    internal class Rule_FileMove : RuleBase
    {
        public string TargetPath { get; set; }

        public string EvacuateDirName
        {
            get
            {
                return $"{this.Name}_{this.Index:0000}";
            }
        }

        public void ToHidden()
        {
            var fileName = Path.GetFileName(this.TargetPath);
            var sourcePath = this.TargetPath;
            var destinationPath = Path.Combine(this.EvacuateDirName, fileName);
            if (File.Exists(this.TargetPath))
            {
                File.Move(sourcePath, destinationPath);
            }
            else if (Directory.Exists(this.TargetPath))
            {
                Directory.Move(sourcePath, destinationPath);
            }
            else
            {
                // ファイルもしくはディレクトリも存在しない場合
            }
        }

        public void ToVisible()
        {
            var fileName = Path.GetFileName(this.TargetPath);
            var sourcePath = Path.Combine(this.EvacuateDirName, fileName);
            var destinationPath = this.TargetPath;
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath);
            }
            else if (Directory.Exists(sourcePath))
            {
                FileSystem.CopyDirectory(sourcePath, destinationPath);
            }
            else
            {
                // ファイルもしくはディレクトリも存在しない場合
            }
        }
    }
}
