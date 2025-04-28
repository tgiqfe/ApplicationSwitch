using ApplicationSwitch.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Sample.CreateSample
{
    [SupportedOSPlatform("windows")]
    internal class RuleFile01
    {
        public static AppRoot Create()
        {
            return new AppRoot()
            {
                Config = new AppConfig()
                {
                    Metadata = new AppConfigMetadata()
                    {
                        Name = "Sample-RuleFile01",
                        Description = "Sample, 7-zip shortcut file evacuation."
                    },
                    Target = new AppConfigTarget()
                    {
                        EnableTargets = "ClientPCA01, ClientPCA02, ClientPCA03",
                        DisableTargets = "ClientPCB01, ClientPCB02, ClientPCB03",
                    },
                    Rule = new AppConfigRule()
                    {
                        Rules = new List<AppRuleTemplate>()
                        {
                            new AppRuleTemplate()
                            {
                                Action = "File",
                                Name = "Sample_(7-Zip)_1",
                                TargetPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\7-Zip"
                            },
                            new AppRuleTemplate()
                            {
                                Action = "File",
                                Name = "Sample_(7-Zip)_2",
                                TargetPath = @"C:\Users\Public\Desktop\App\7-Zip File Manager.lnk",
                                RemoveEmptyParent = "true",
                            }
                        }
                    }
                }
            };
        }
    }
}
