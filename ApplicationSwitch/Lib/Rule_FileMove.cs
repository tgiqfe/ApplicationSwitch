using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    internal class Rule_FileMove : RuleBase
    {
        public static readonly string[] TYPE_NAME_PATTERN = { "FileMove", "Move" };

        [YamlIgnore]
        public string TargetFilePath { get; set; }

        [YamlIgnore]
        public string EvacuateDirName
        {
            get
            {
                return $"{this.Name}_{this.Index:0000}";
            }
        }

        public override void ToHidden()
        {
            var fileName = Path.GetFileName(this.TargetFilePath);
            var sourcePath = this.TargetFilePath;
            var destinationPath = Path.Combine(this.EvacuateDirName, fileName);
            if (File.Exists(this.TargetFilePath))
            {
                File.Move(sourcePath, destinationPath);
            }
            else if (Directory.Exists(this.TargetFilePath))
            {
                Directory.Move(sourcePath, destinationPath);
            }
            else
            {
                // ファイルもしくはディレクトリも存在しない場合
            }
        }

        public override void ToVisible()
        {
            var fileName = Path.GetFileName(this.TargetFilePath);
            var sourcePath = Path.Combine(this.EvacuateDirName, fileName);
            var destinationPath = this.TargetFilePath;
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
