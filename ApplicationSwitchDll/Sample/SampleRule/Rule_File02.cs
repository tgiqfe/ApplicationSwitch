using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using System.Reflection;

namespace ApplicationSwitch.Sample.SampleRule
{
    internal class Rule_File02
    {
        public static string Create()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            var appRoot = new AppRoot()
            {
                Config = new AppConfig()
                {
                    Metadata = new AppConfigMetadata()
                    {
                        Name = "Rule_File02",
                        Description = "Sample Rule File",
                        Version = version,
                    },
                    Target = new AppConfigTarget()
                    {
                        EnableTargets = "ClientPCA*, ClientPCB*, ClientPCC*\r\nAppPCA*, AppPCB*, AppPCC*, AppPCD*",
                        DisableTargets = "ExampleA-*, ExampleB-*, ExampleC-*",
                        PrimaryTarget = "Disable"
                    },
                    Rule = new AppConfigRule()
                    {
                        Rules = new List<AppRuleTemplate>()
                        {
                            new AppRuleTemplate()
                            {
                                Action = "File",
                                Name = "Rule01",
                                TargetPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\7-Zip",
                            },
                            new AppRuleTemplate()
                            {
                                Action = "File",
                                Name = "Rule02",
                                TargetPath = @"C:\Users\Public\Desktop\7-zip.lnk",
                                RemoveEmptyParent = "true",
                            }
                        }
                    }
                }
            };

            return Functions.ToText(appRoot);
        }
    }
}
