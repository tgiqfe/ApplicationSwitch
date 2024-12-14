using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Manifest
{
    internal class Switch_Permission : SwitchBase
    {
        public static readonly string[] TYPE_NAME_PATTERN = { "Permission", "acl", "access" };

        public class AccessRule
        {
            public string Account { get; set; }
            public FileSystemRights Rights { get; set; }
            public AccessControlType IsAllow { get; set; }
        }

        public string TargetFilePath { get; set; }
        public bool IsInherited { get; set; }
        public string[] AdminAccount { get; set; }
        public List<AccessRule> AccessRules { get; set; }

        public override void ToHidden()
        {
            if (File.Exists(this.TargetFilePath))
            {
                var fi = new FileInfo(this.TargetFilePath);
                FileSecurity security = fi.GetAccessControl();

                security.SetAccessRuleProtection(true, false);
                foreach (FileSystemAccessRule rule in security.GetAccessRules(true, false, typeof(NTAccount)))
                {
                    security.RemoveAccessRule(rule);
                }
                foreach (var name in this.AdminAccount)
                {
                    FileSystemAccessRule fsar = new FileSystemAccessRule(
                        new NTAccount(name),
                        FileSystemRights.FullControl,
                        AccessControlType.Allow);
                    security.AddAccessRule(fsar);
                }
                fi.SetAccessControl(security);
            }
            else if (Directory.Exists(this.TargetFilePath))
            {
                var di = new DirectoryInfo(this.TargetFilePath);
                DirectorySecurity security = di.GetAccessControl();

                security.SetAccessRuleProtection(true, false);
                foreach (FileSystemAccessRule rule in security.GetAccessRules(true, false, typeof(NTAccount)))
                {
                    security.RemoveAccessRule(rule);
                }
                foreach (var name in this.AdminAccount)
                {
                    FileSystemAccessRule fsar = new FileSystemAccessRule(
                        new NTAccount(name),
                        FileSystemRights.FullControl,
                        InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow);
                    security.AddAccessRule(fsar);
                }
            }
            else
            {
                // ファイルもしくはディレクトリも存在しない場合
            }
        }

        public override void ToVisible()
        {
            if (File.Exists(this.TargetFilePath))
            {
                var fi = new FileInfo(this.TargetFilePath);
                FileSecurity security = fi.GetAccessControl();

                security.SetAccessRuleProtection(!this.IsInherited, false);
                if (this.AccessRules?.Count > 0)
                {
                    foreach (FileSystemAccessRule rule in security.GetAccessRules(true, false, typeof(NTAccount)))
                    {
                        security.RemoveAccessRule(rule);
                    }
                    foreach (var rule in this.AccessRules)
                    {
                        FileSystemAccessRule fsar = new FileSystemAccessRule(
                            new NTAccount(rule.Account),
                            rule.Rights,
                            rule.IsAllow);
                        security.AddAccessRule(fsar);
                    }
                }
                fi.SetAccessControl(security);
            }
            else if (Directory.Exists(this.TargetFilePath))
            {
                var di = new DirectoryInfo(this.TargetFilePath);
                DirectorySecurity security = di.GetAccessControl();

                security.SetAccessRuleProtection(!this.IsInherited, false);
                if (this.AccessRules?.Count > 0)
                {
                    foreach (FileSystemAccessRule rule in security.GetAccessRules(true, false, typeof(NTAccount)))
                    {
                        security.RemoveAccessRule(rule);
                    }
                    foreach (var rule in this.AccessRules)
                    {
                        FileSystemAccessRule fsar = new FileSystemAccessRule(
                            new NTAccount(rule.Account),
                            rule.Rights,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.None,
                            rule.IsAllow);
                        security.AddAccessRule(fsar);
                    }
                }
                di.SetAccessControl(security);
            }
            else
            {
                // ファイルもしくはディレクトリも存在しない場合
            }
        }
    }
}
