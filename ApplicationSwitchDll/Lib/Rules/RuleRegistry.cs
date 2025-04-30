using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleRegistry : RuleBase
    {
        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }

        const string _BACKUP_KEY_NAME = "registry_backup.reg";
        const string _BACKUP_PARAM_NAME = "registry_backup.json";
        private string EvacuateFilePath
        {
            get
            {
                return Path.Combine(
                    this.EvacuateRulePath,
                    this.RegistryParam == null ? _BACKUP_KEY_NAME : _BACKUP_PARAM_NAME);
            }
        }

        public override void Initialize()
        {
            this.Enabled =
                !string.IsNullOrEmpty(this.Name) &&
                !string.IsNullOrEmpty(this.RegistryKey);
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

        private static string RegistryValueKindToString(RegistryValueKind valueKind)
        {
            return valueKind switch
            {
                RegistryValueKind.String => "REG_SZ",
                RegistryValueKind.DWord => "REG_DWORD",
                RegistryValueKind.QWord => "REG_QWORD",
                RegistryValueKind.Binary => "REG_BINARY",
                RegistryValueKind.MultiString => "REG_MULTI_SZ",
                RegistryValueKind.ExpandString => "REG_EXPAND_SZ",
                _ => "Unknown",
            };
        }

        private static object StringToRegistryValue(string text, RegistryValueKind type)
        {
            return type switch
            {
                RegistryValueKind.String => text,
                RegistryValueKind.DWord => int.Parse(text),
                RegistryValueKind.QWord => long.Parse(text),
                RegistryValueKind.Binary => StringToRegBinary(text),
                RegistryValueKind.MultiString => text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries),
                RegistryValueKind.ExpandString => text,
                _ => null,
            };

            byte[] StringToRegBinary(string val)
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

        private static string RegistryValueToString(RegistryKey regKey, string name, RegistryValueKind valueKind, bool noResolv)
        {
            return valueKind switch
            {
                RegistryValueKind.String => regKey.GetValue(name) as string,
                RegistryValueKind.DWord => regKey.GetValue(name) as string,
                RegistryValueKind.QWord => regKey.GetValue(name) as string,
                RegistryValueKind.Binary => BitConverter.ToString(regKey.GetValue(name) as byte[]).Replace("-", "").ToString(),
                RegistryValueKind.MultiString => string.Join("\r\n", regKey.GetValue(name) as string[]),
                RegistryValueKind.ExpandString => noResolv ?
                regKey.GetValue(name, "", RegistryValueOptions.DoNotExpandEnvironmentNames) as string :
                    regKey.GetValue(name) as string,
                _ => null,
            };
        }

        private static bool RegistryExists(RegistryKey regKey, string name = null)
        {
            if (regKey == null) return false;
            return name == null ?
                true :
                regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private static bool RegistryExists(string path, string name = null)
        {
            using (var regKey = GetRegistryKey(path, false, false))
            {
                return RegistryExists(regKey, name);
            }
        }

        #endregion
        #region Registry param backup class

        internal class RegistryParamBackup
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
        }

        #endregion

        /// <summary>
        /// Registry key/param restore to source.
        /// </summary>
        public override void EnableProcess()
        {
            if (this.RegistryParam == null)
            {
                if (File.Exists(this.EvacuateFilePath) && !RegistryExists(this.RegistryKey))
                {
                    using (var proc = new Process())
                    {
                        proc.StartInfo.FileName = "cmd";
                        proc.StartInfo.Arguments = $"/c reg import \"{this.EvacuateFilePath}\"";
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.Start();
                        proc.WaitForExit();
                    }
                }
            }
            else
            {
                if (File.Exists(this.EvacuateFilePath) && !RegistryExists(this.RegistryKey, this.RegistryParam))
                {
                    var backup = JsonSerializer.Deserialize<RegistryParamBackup>(File.ReadAllText(this.EvacuateFilePath, Encoding.UTF8));
                    using (var regKey = GetRegistryKey(this.RegistryKey, true, true))
                    {
                        regKey.SetValue(
                            backup.Name,
                            StringToRegistryValue(backup.Value, StringToValueKind(backup.Type)),
                            StringToValueKind(backup.Type));
                    }
                }
            }

            EndProcess(isEnableProcess: true);
        }

        /// <summary>
        /// Registry key/param move to evacuate path.
        /// </summary>
        public override void DisableProcess()
        {
            if (!Directory.Exists(this.EvacuateRulePath))
            {
                Directory.CreateDirectory(this.EvacuateRulePath);
            }

            if (this.RegistryParam == null)
            {
                //  evacuate registry key.
                if (!RegistryExists(this.RegistryKey))
                {
                    return;
                }
                using (var proc = new Process())
                {
                    proc.StartInfo.FileName = "cmd";
                    proc.StartInfo.Arguments = $"/c reg export \"{this.RegistryKey}\" \"{this.EvacuateFilePath}\" /y";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    proc.WaitForExit();
                }
                using (var parentKey = GetRegistryKey(Path.GetDirectoryName(this.RegistryKey), false, true))
                {
                    parentKey.DeleteSubKeyTree(Path.GetFileName(this.RegistryKey));
                }
            }
            else
            {
                using (var regKey = GetRegistryKey(this.RegistryKey, false, true))
                {
                    if (!RegistryExists(regKey, this.RegistryParam))
                    {
                        return;
                    }

                    var valueKind = regKey.GetValueKind(this.RegistryParam);
                    var content = JsonSerializer.Serialize(
                        new RegistryParamBackup()
                        {
                            Key = this.RegistryKey,
                            Name = this.RegistryParam,
                            Type = RegistryValueKindToString(valueKind),
                            Value = RegistryValueToString(regKey, this.RegistryParam, valueKind, false)
                        }, new JsonSerializerOptions() { WriteIndented = true });
                    File.WriteAllText(this.EvacuateFilePath, content, Encoding.UTF8);

                    regKey.DeleteValue(this.RegistryParam);
                }
            }

            EndProcess(isEnableProcess: false);
        }
    }
}
