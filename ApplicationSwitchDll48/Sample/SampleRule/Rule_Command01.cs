using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Sample.SampleRule
{
    internal class Rule_Command01
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
                        Name = "Rule_Command01",
                        Description = "Sample Rule Command",
                        Version = version,
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
                                Action = "Command",
                                Name = "Rule01",
                                EnableCommand = @"cmd.exe /c echo 1 >> %USERPROFILE%\Desktop\test.txt",
                                DisableCommand = @"cmd.exe /c echo 0 >> %USERPROFILE%\Desktop\test.txt",
                            }
                        }
                    }
                }
            };

            return Functions.ToText(appRoot);
        }
    }
}
