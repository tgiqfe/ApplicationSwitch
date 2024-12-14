using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Manifest
{
    internal class Switch_FileMove : SwitchBase
    {
        public static readonly string[] TYPE_NAME_PATTERN = { "FileMove", "Move" };

        public string TargetFilePath { get; set; }
        public string EvacuateDirPath { get; set; }

        public override void ToHidden()
        {
            var fileName = Path.GetFileName(TargetFilePath);
            var sourcePath = TargetFilePath;
            var destinationPath = Path.Combine(EvacuateDirPath, fileName);
            if (File.Exists(TargetFilePath))
            {
                File.Move(sourcePath, destinationPath, true);
            }
            else if (Directory.Exists(TargetFilePath))
            {
                if (Directory.Exists(destinationPath))
                {
                    Directory.Delete(destinationPath, true);
                    Directory.Move(sourcePath, destinationPath);
                }
            }
            else
            {
                // ファイルもしくはディレクトリも存在しない場合
            }
        }

        public override void ToVisible()
        {
            var fileName = Path.GetFileName(TargetFilePath);
            var sourcePath = Path.Combine(EvacuateDirPath, fileName);
            var destinationPath = TargetFilePath;
            if (File.Exists(sourcePath))
            {
                if (!File.Exists(destinationPath))
                {
                    File.Copy(sourcePath, destinationPath);
                }
            }
            else if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destinationPath))
                {
                    FileSystem.CopyDirectory(sourcePath, destinationPath);
                }
            }
            else
            {
                // ファイルもしくはディレクトリも存在しない場合
            }
        }
    }
}
