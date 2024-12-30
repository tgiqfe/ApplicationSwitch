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

        private static (string, string) GetCommandText(string commandText)
        {
            string command = commandText.Trim();
            if (command.Trim().StartsWith("\""))
            {
                return (
                    command.Substring(1, command.IndexOf("\"", 1) - 1).Trim(),
                    command.Substring(command.IndexOf("\"", 1) + 1).Trim());
            }
            else if (command.Trim().Contains(" "))
            {
                return (
                    command.Substring(0, command.IndexOf(" ")).Trim(),
                    command.Substring(command.IndexOf(" ") + 1).Trim());
            }
            return (command, "");
        }

        private static Process GetScriptProcess(string scriptPath)
        {
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
                (var command, var arguments) = GetCommandText(this.EnableCommand);
                using (var proc = new Process())
                {
                    proc.StartInfo.FileName = command;
                    proc.StartInfo.Arguments = arguments;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    proc.WaitForExit();
                }
            }
            else if (!string.IsNullOrEmpty(this.EnableScript))
            {
                Logger.WriteLine($"RuleCommand, Execute enable Script => {this.EnableScript}", 4);
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
                (var command, var arguments) = GetCommandText(this.DisableCommand);
                using (var proc = new Process())
                {
                    proc.StartInfo.FileName = command;
                    proc.StartInfo.Arguments = arguments;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    proc.WaitForExit();
                }
            }
            else if (!string.IsNullOrEmpty(this.DisableScript))
            {
                Logger.WriteLine($"RuleCommand, Execute disable Script => {this.DisableScript}", 4);
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
