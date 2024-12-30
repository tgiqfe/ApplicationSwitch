using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    internal class RuleRegistry : RuleBase
    {
        const string evacuateKeyName = "registry_backup.reg";
        const string evacuateParamName = "registry_backup.json";

        public string RegistryKey { get; set; }
        public string RegistryParam { get; set; }

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
        /// Registry set process for Enable target.
        /// </summary>
        public override void EnableProcess()
        {
            if (this.RegistryParam == null)
            {
                var source = Path.Combine(this.AppEvacuatePath, evacuateKeyName);

                //  Evacuate registry key.
                if (File.Exists(source) && !RegistryExists(this.RegistryKey))
                {
                    Logger.WriteLine($"RuleRegistry, Registry key restore. {this.RegistryKey}", 4);
                    using (var proc = new Process())
                    {
                        proc.StartInfo.FileName = "reg.exe";
                        proc.StartInfo.Arguments = $"import \"{source}\"";
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.Start();
                        proc.WaitForExit();
                    }
                }
            }
            else
            {
                var source = Path.Combine(this.AppEvacuatePath, evacuateParamName);
                string nameForLog = string.IsNullOrEmpty(this.RegistryParam) ? "(Default)" : this.RegistryParam;

                //  Evacuate registry parameter.
                if (File.Exists(source) && !RegistryExists(this.RegistryKey, this.RegistryParam))
                {
                    Logger.WriteLine($"RuleRegistry, Registry param restore. {this.RegistryKey} / {nameForLog}", 4);
                    var backup = JsonSerializer.Deserialize<RegistryParamBackup>(File.ReadAllText(source));
                    using (var regKey = GetRegistryKey(this.RegistryKey, true, true))
                    {
                        regKey.SetValue(
                            backup.Name,
                            StringToRegistryValue(backup.Value, StringToValueKind(backup.Type)),
                            StringToValueKind(backup.Type));
                    }
                }
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

            if (this.RegistryParam == null)
            {
                var destination = Path.Combine(this.AppEvacuatePath, evacuateKeyName);

                //  Evacuate registry key.
                using (var regKey = GetRegistryKey(this.RegistryKey))
                {
                    if (!RegistryExists(regKey))
                    {
                        Logger.WriteLine($"RuleRegistry, Registry key not found. {this.RegistryKey}", 4);
                        return;
                    }
                }
                using (var proc = new Process())
                {
                    Logger.WriteLine($"RuleRegistry, Registry key evacuate. {this.RegistryKey}", 4);
                    proc.StartInfo.FileName = "cmd";
                    proc.StartInfo.Arguments = $"/c reg export \"{this.RegistryKey}\" \"{destination}\" /y";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    proc.WaitForExit();
                }
                using (var regKey = GetRegistryKey(Path.GetDirectoryName(this.RegistryKey), false, true))
                {
                    Logger.WriteLine($"RuleRegistry, Registry key remove. {this.RegistryKey}", 4);
                    regKey.DeleteSubKeyTree(Path.GetFileName(this.RegistryKey));
                }
            }
            else
            {
                var destination = Path.Combine(this.AppEvacuatePath, evacuateParamName);
                string nameForLog = string.IsNullOrEmpty(this.RegistryParam) ? "(Default)" : this.RegistryParam;

                //  Evacuate registry parameter.
                using (var regKey = GetRegistryKey(this.RegistryKey))
                {
                    if (!RegistryExists(regKey, this.RegistryParam))
                    {   
                        Logger.WriteLine($"RuleRegistry, Registry key or param not found. {this.RegistryKey} / {nameForLog}", 4);
                        return;
                    }

                    Logger.WriteLine($"RuleRegistry, Registry param evacuate. {this.RegistryKey} / {nameForLog}", 4);
                    var valueKind = regKey.GetValueKind(this.RegistryParam);
                    var content = JsonSerializer.Serialize(
                        new RegistryParamBackup()
                        {
                            Key = this.RegistryKey,
                            Name = this.RegistryParam,
                            Type = RegistryValueKindToString(valueKind),
                            Value = RegistryValueToString(regKey, this.RegistryParam, valueKind, false)
                        }, new JsonSerializerOptions() { WriteIndented = true });
                    File.WriteAllText(destination, content);
                }
                using (var regKey = GetRegistryKey(this.RegistryKey, false, true))
                {
                    Logger.WriteLine($"RuleRegistry, Registry param remove. {this.RegistryKey} / {nameForLog}", 4);
                    regKey.DeleteValue(this.RegistryParam);
                }
            }
        }
    }
}
