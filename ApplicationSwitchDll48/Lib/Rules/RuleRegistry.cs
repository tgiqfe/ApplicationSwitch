using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            RegistryKey root = null;
            if (candidate_HKCR.Any(x => x.Equals(rootName, StringComparison.OrdinalIgnoreCase)))
            {
                root = Registry.ClassesRoot;
            }
            else if (candidate_HKCU.Any(x => x.Equals(rootName, StringComparison.OrdinalIgnoreCase)))
            {
                root = Registry.CurrentUser;
            }
            else if (candidate_HKLM.Any(x => x.Equals(rootName, StringComparison.OrdinalIgnoreCase)))
            {
                root = Registry.LocalMachine;
            }
            else if (candidate_HKU.Any(x => x.Equals(rootName, StringComparison.OrdinalIgnoreCase)))
            {
                root = Registry.Users;
            }
            else if (candidate_HKCC.Any(x => x.Equals(rootName, StringComparison.OrdinalIgnoreCase)))
            {
                root = Registry.CurrentConfig;
            }

            return isCreate ?
                root.CreateSubKey(keyName, writable) :
                root.OpenSubKey(keyName, writable);
        }

        private static RegistryValueKind StringToValueKind(string text)
        {
            if (candidate_string.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
            {
                return RegistryValueKind.String;
            }
            else if (candidate_dword.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
            {
                return RegistryValueKind.DWord;
            }
            else if (candidate_qword.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
            {
                return RegistryValueKind.QWord;
            }
            else if (candidate_binary.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
            {
                return RegistryValueKind.Binary;
            }
            else if (candidate_multi.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
            {
                return RegistryValueKind.MultiString;
            }
            else if (candidate_expand.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
            {
                return RegistryValueKind.ExpandString;
            }
            else
            {
                return RegistryValueKind.Unknown;
            }
        }

        private static string RegistryValueKindToString(RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case RegistryValueKind.String:
                    return "REG_SZ";
                case RegistryValueKind.DWord:
                    return "REG_DWORD";
                case RegistryValueKind.QWord:
                    return "REG_QWORD";
                case RegistryValueKind.Binary:
                    return "REG_BINARY";
                case RegistryValueKind.MultiString:
                    return "REG_MULTI_SZ";
                case RegistryValueKind.ExpandString:
                    return "REG_EXPAND_SZ";
                default:
                    return "Unknown";
            }
        }

        private static object StringToRegistryValue(string text, RegistryValueKind type)
        {
            switch (type)
            {
                case RegistryValueKind.String:
                    return text;
                case RegistryValueKind.DWord:
                    return int.Parse(text);
                case RegistryValueKind.QWord:
                    return long.Parse(text);
                case RegistryValueKind.Binary:
                    return __StringToRegBinary(text);
                case RegistryValueKind.MultiString:
                    return text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                case RegistryValueKind.ExpandString:
                    return text;
                default:
                    return null;
            }

            byte[] __StringToRegBinary(string val)
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
            switch (valueKind)
            {
                case RegistryValueKind.String:
                    return regKey.GetValue(name) as string;
                case RegistryValueKind.DWord:
                    return regKey.GetValue(name) as string;
                case RegistryValueKind.QWord:
                    return regKey.GetValue(name) as string;
                case RegistryValueKind.Binary:
                    return BitConverter.ToString(regKey.GetValue(name) as byte[]).Replace("-", "").ToString();
                case RegistryValueKind.MultiString:
                    return string.Join("\r\n", regKey.GetValue(name) as string[]);
                case RegistryValueKind.ExpandString:
                    return noResolv ?
                        regKey.GetValue(name, "", RegistryValueOptions.DoNotExpandEnvironmentNames) as string :
                        regKey.GetValue(name) as string;
                default:
                    return null;
            }
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
