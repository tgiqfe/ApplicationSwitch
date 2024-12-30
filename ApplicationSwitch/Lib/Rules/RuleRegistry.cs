using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleRegistry : RuleBase
    {
        const string evacuateKeyName = "registry_backup.reg";
        const string evacuateParamName = "registry_backup.txt";

        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }
        public RegistryValueKind RegistryType { get; set; }

        public RuleRegistry(string name, string evacuate, string registryKey, string registryParam)
        {
            this.Name = name;
            this.AppEvacuate = evacuate;
            this.RegistryKey = registryKey;
            this.RegistryParam = registryParam;

            if (string.IsNullOrEmpty(this.Name))
            {
                Logger.WriteLine("RuleRegistry, Name is empty.");
                return;
            }
            Logger.WriteLine($"RuleRegistry, Rule name => {this.Name}");

            if (string.IsNullOrEmpty(this.RegistryKey))
            {
                Logger.WriteLine($"RuleRegistry, Registry key is emply.");
                return;
            }

            this.Enabled = true;
        }

        #region Registry control methods.

        private static readonly string[] candidate_string = { "String", "reg_sz", "sz" };
        private static readonly string[] candidate_dword = { "Dword", "reg_dword", "dword" };
        private static readonly string[] candidate_qword = { "Qword", "reg_qword", "qword" };
        private static readonly string[] candidate_binary = { "Binary", "reg_binary", "binary" };
        private static readonly string[] candidate_multi = { "MultiString", "reg_multi_sz", "multi", "multi_sz" };
        private static readonly string[] candidate_expand = { "ExpandString", "reg_expand_sz", "expand", "expand_sz" };

        private static readonly string[] candidate_HKCR = { "HKCR", "HKEY_CLASSES_ROOT", "HKCR:" };
        private static readonly string[] candidate_HKCU = { "HKCU", "HKEY_CURRENT_USER", "HKCU:" };
        private static readonly string[] candidate_HKLM = { "HKLM", "HKEY_LOCAL_MACHINE", "HKLM:" };
        private static readonly string[] candidate_HKU = { "HKU", "HKEY_USERS", "HKU:" };
        private static readonly string[] candidate_HKCC = { "HKCC", "HKEY_CURRENT_CONFIG", "HKCC:" };

        private static RegistryKey GetRegistryKey(string path, bool isCreate = false, bool writable = false)
        {
            string rootName = path.Substring(0, path.IndexOf("\\"));
            string keyName = path.Substring(path.IndexOf("\\") + 1);

            var root = rootName switch
            {
                string s when candidate_HKCR.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => Registry.ClassesRoot,
                string s when candidate_HKCU.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => Registry.CurrentUser,
                string s when candidate_HKLM.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => Registry.LocalMachine,
                string s when candidate_HKU.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => Registry.Users,
                string s when candidate_HKCC.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => Registry.CurrentConfig,
                _ => null,
            };
            return isCreate ?
                root.CreateSubKey(keyName, writable) :
                root.OpenSubKey(keyName, writable);
        }


        private static RegistryValueKind StringToValueKind(string text)
        {
            return text switch
            {
                string s when candidate_string.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => RegistryValueKind.String,
                string s when candidate_dword.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => RegistryValueKind.DWord,
                string s when candidate_qword.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => RegistryValueKind.QWord,
                string s when candidate_binary.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => RegistryValueKind.Binary,
                string s when candidate_multi.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => RegistryValueKind.MultiString,
                string s when candidate_expand.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)) => RegistryValueKind.ExpandString,
                _ => RegistryValueKind.Unknown,
            };
        }

        private static object StringToObject(string text, RegistryValueKind type)
        {
            return type switch
            {
                RegistryValueKind.String => text,
                RegistryValueKind.DWord => int.Parse(text),
                RegistryValueKind.QWord => long.Parse(text),
                RegistryValueKind.Binary => stringToRegBinary(text),
                RegistryValueKind.MultiString => text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries),
                RegistryValueKind.ExpandString => text,
                _ => null,
            };

            byte[] stringToRegBinary(string val)
            {
                if (Regex.IsMatch(val, @"^[0-9a-fA-F]+$"))
                {
                    List<byte> bytes = new List<byte>();
                    for (int i = 0; i < val.Length / 2; i++)
                    {
                        bytes.Add(Convert.ToByte(val.Substring(i * 2, 2), 16));
                    }
                    return bytes.ToArray();
                }
                return new byte[0] { };
            }
        }

        #endregion

        /// <summary>
        /// Registry set process for Enable target.
        /// </summary>
        public override void EnableProcess()
        {
            if (this.RegistryParam == null)
            {
                //  Evacuate registry key.

            }
            else
            {
                //  Evacuate registry parameter.
            }
        }

        /// <summary>
        /// Registry remove process for Disable target.
        /// </summary>
        public override void DisableProcess()
        {
            if (!Directory.Exists(this.AppEvacuatePath))
            {
                Directory.CreateDirectory(this.AppEvacuatePath);
            }

            var destination = Path.Combine(this.AppEvacuatePath, evacuateKeyName);
            if (this.RegistryParam == null)
            {
                //  Evacuate registry key.
                using (var regKey = GetRegistryKey(this.RegistryKey))
                {
                    if (regKey == null)
                    {
                        Logger.WriteLine($"RuleRegistry, Registry key not found. {this.RegistryKey}");
                        return;
                    }
                }
                using (var proc = new Process())
                {
                    proc.StartInfo.FileName = "reg.exe";
                    proc.StartInfo.Arguments = $"export \"{this.RegistryKey}\" \"{evacuateKeyName}\"";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    proc.WaitForExit();
                }
            }
            else
            {
                //  Evacuate registry parameter.
                using (var regKey = GetRegistryKey(this.RegistryKey))
                {
                    if (regKey == null)
                    {
                        Logger.WriteLine($"RuleRegistry, Registry key not found. {this.RegistryKey}");
                        return;
                    }







                    var value = regKey.GetValue(this.RegistryParam);
                    if (value == null)
                    {
                        Logger.WriteLine($"RuleRegistry, Registry parameter not found. {this.RegistryParam}");
                    }
                    else
                    {
                        var type = regKey.GetValueKind(this.RegistryParam);
                        using (var regKeyEvacuate = GetRegistryKey(this.AppEvacuatePath, true, true))
                        {
                            regKeyEvacuate.SetValue(this.RegistryParam, value, type);
                        }
                        regKey.DeleteValue(this.RegistryParam);
                    }
                }
            }
        }
    }
}
