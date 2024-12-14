using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Manifest
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
                return $"{Name}_{Index:0000}";
            }
        }

        public override void ToHidden()
        {
            var fileName = Path.GetFileName(TargetFilePath);
            var sourcePath = TargetFilePath;
            var destinationPath = Path.Combine(EvacuateDirName, fileName);
            if (File.Exists(TargetFilePath))
            {
                File.Move(sourcePath, destinationPath);
            }
            else if (Directory.Exists(TargetFilePath))
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
            var fileName = Path.GetFileName(TargetFilePath);
            var sourcePath = Path.Combine(EvacuateDirName, fileName);
            var destinationPath = TargetFilePath;
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
