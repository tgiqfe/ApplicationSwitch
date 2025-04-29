using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleCommand : RuleBase
    {
        const string _RULE_NAME = "RuleCommand";

        public string EnableCommand { get; set; }
        public string DisableCommand { get; set; }
        public string EnableScript { get; set; }
        public string DisableScript { get; set; }

        public RuleCommand(string name, string enCmd, string disCmd, string enScript, string disScript)
        {
            this.Name = name;
            this.EnableCommand = enCmd;
            this.DisableCommand = disCmd;
            this.EnableScript = enScript;
            this.DisableScript = disScript;

            //  Name parameter checking.
            if (string.IsNullOrEmpty(this.Name))
            {
                return;
            }
            if (string.IsNullOrEmpty(this.EnableCommand) && string.IsNullOrEmpty(this.DisableCommand) &&
                string.IsNullOrEmpty(this.EnableScript) && string.IsNullOrEmpty(this.DisableScript))
            {

                return;
            }

            this.Enabled = true;
        }

        #region command text functions

        private static Process GetCommandProcess(string commandText)
        {
            string command = commandText.Trim();
            if (command.Trim().StartsWith("\""))
            {
                return new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = command.Substring(1, command.IndexOf("\"", 1) - 1).Trim(),
                        Arguments = command.Substring(command.IndexOf("\"", 1) + 1).Trim(),
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
            }
            else if (command.Trim().Contains(" "))
            {
                return new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = command.Substring(0, command.IndexOf(" ")).Trim(),
                        Arguments = command.Substring(command.IndexOf(" ") + 1).Trim(),
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
            }
            else
            {
                return new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = command,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
            }

        }

        private static Process GetScriptProcess(string scriptPathText)
        {
            string scriptPath = scriptPathText.Trim().Trim('\"').Trim('\'');
            string extension = Path.GetExtension(scriptPath).ToLower();
            return extension switch
            {
                ".bat" or ".cmd" => new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c \"{scriptPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                },
                ".ps1" => new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-ExecutionPolicy Unrestricted -File \"{scriptPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                },
                ".exe" => new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = scriptPath,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                },
                _ => null,
            };
        }

        #endregion

        public override void EnableProcess()
        {
            if (!string.IsNullOrEmpty(this.EnableCommand))
            {
                using (var proc = GetCommandProcess(this.EnableCommand))
                {
                    if (proc != null)
                    {
                        proc.Start();
                        proc.WaitForExit();
                    }
                }
            }
            else if (!string.IsNullOrEmpty(this.EnableScript))
            {
                if (!File.Exists(this.EnableScript))
                {
                    return;
                }
                using (var proc = GetScriptProcess(this.EnableScript))
                {
                    if (proc != null)
                    {
                        proc.Start();
                        proc.WaitForExit();
                    }
                }
            }
        }

        public override void DisableProcess()
        {
            if (!string.IsNullOrEmpty(this.DisableCommand))
            {
                using (var proc = GetCommandProcess(this.DisableCommand))
                {
                    if (proc != null)
                    {
                        proc.Start();
                        proc.WaitForExit();
                    }
                }
            }
            else if (!string.IsNullOrEmpty(this.DisableScript))
            {
                if (!File.Exists(this.DisableScript))
                {
                    return;
                }
                using (var proc = GetScriptProcess(this.DisableScript))
                {
                    if (proc != null)
                    {
                        proc.Start();
                        proc.WaitForExit();
                    }
                }
            }
        }
    }
}
