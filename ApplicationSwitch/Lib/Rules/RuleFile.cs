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
        public string Target { get; set; }

        private string _evacuatePath { get { return Path.Combine(this.EvacuatePath, Name); } }

        public override void EnableProcess()
        {
            var parent = Path.GetDirectoryName(this.Target);
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }

            var source = Path.Combine(_evacuatePath, Path.GetFileName(this.Target));
            if (File.Exists(source) && !File.Exists(this.Target))
            {
                //  File evacuate.
                FileSystem.MoveFile(source, this.Target, true);
            }
            else if (Directory.Exists(source) && Directory.Exists(this.Target))
            {
                //  Directory evacuate.
                FileSystem.MoveDirectory(source, this.Target, true);
            }
            else
            {
                //  move not eligible,
            }
        }

        public override void DisableProcess()
        {
            if (!Directory.Exists(_evacuatePath))
            {
                Directory.CreateDirectory(_evacuatePath);
            }

            var destination = Path.Combine(_evacuatePath, Path.GetFileName(this.Target));
            if (File.Exists(this.Target))
            {
                //  File evacuate.
                FileSystem.MoveFile(this.Target, _evacuatePath, true);
            }
            else if (Directory.Exists(this.Target))
            {
                //  Directory evacuate.
                FileSystem.MoveDirectory(this.Target, _evacuatePath, true);
            }
            else
            {
                //  Target file or directory not found.
            }
        }
    }
}
