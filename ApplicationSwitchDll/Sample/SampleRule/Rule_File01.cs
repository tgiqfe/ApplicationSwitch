using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Sample.SampleRule
{
    internal class Rule_File01
    {
        public static string Create()
        {
            var appRoot = new AppRoot()
            {
                Config = new AppConfig()
                {
                    Metadata = new AppConfigMetadata()
                    {
                        Name = "Rule_File01",
                        Description = "Sample Rule File",
                        Version = "1.0.0.1"
                    },
                    Target = new AppConfigTarget()
                    {
                        EnableTargets = "ClientPC-A01, ClientPC-A02, ClientPC-A03",
                        DisableTargets = "ClientPC-B01, ClientPC-B02, ClientPC-B03"
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
