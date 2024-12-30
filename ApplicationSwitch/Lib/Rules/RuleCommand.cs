using System.Diagnostics;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleCommand : RuleBase
    {
        public string EnableCommand { get; set; }
        public string DisableCommand { get; set; }
        public string EnableScript { get; set; }
        public string DisableScript { get; set; }

        public RuleCommand(string name, string evacuate, string enCmd, string disCmd, string enScript, string disScript)
        {
            this.Name = name;
            this.AppEvacuate = evacuate;
            this.EnableCommand = enCmd;
            this.DisableCommand = disCmd;
            this.EnableScript = enScript;
            this.DisableScript = disScript;

            if (string.IsNullOrEmpty(this.Name))
            {
                Logger.WriteLine("RuleCommand, Name is empty.");
                return;
            }
            if (string.IsNullOrEmpty(this.EnableCommand) && string.IsNullOrEmpty(this.DisableCommand) &&
                string.IsNullOrEmpty(this.EnableScript) && string.IsNullOrEmpty(this.DisableScript))
            {
                Logger.WriteLine("RuCommand, Enable and Disable command,script are empty.");
                return;
            }
            Logger.WriteLine($"RuleCommand, Rule name => {this.Name}");
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

        /// <summary>
        /// command execute process for Enable target.
        /// </summary>
        public override void EnableProcess()
        {
            if (!string.IsNullOrEmpty(this.EnableCommand))
            {
                Logger.WriteLine($"RuleCommand, Execute enable Command => {this.EnableCommand}", 4);
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
                if(!File.Exists(this.EnableScript))
                {
                    Logger.WriteLine($"RuleCommand, Script file not found => {this.EnableScript}", 4);
                    return;
                }
                Logger.WriteLine($"RuleCommand, Execute enable script => {this.EnableScript}", 4);
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

        /// <summary>
        /// command execute process for Disable target.
        /// </summary>
        public override void DisableProcess()
        {
            if (!string.IsNullOrEmpty(this.DisableCommand))
            {
                Logger.WriteLine($"RunCommand, Execute disable Command => {this.DisableCommand}", 4);
                using (var proc = GetCommandProcess(this.DisableCommand))
                {
                    if(proc != null)
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
                    Logger.WriteLine($"RuleCommand, Script file not found => {this.DisableScript}", 4);
                    return;
                }
                Logger.WriteLine($"RuleCommand, Execute disable script => {this.DisableScript}", 4);
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
