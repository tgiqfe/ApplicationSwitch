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
    internal class Rule_Hidden01
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
                        Name = "Rule_Hidden01",
                        Description = "Sample Rule Hidden",
                        Version = version,
                    },
                    Target = new AppConfigTarget()
                    {
                        EnableTargets = "ClientPC-A01~05, ClientPC-B11~20",
                        DisableTargets = "ClientPC-B01~06, ClientPC-A11~20"
                    },
                    Rule = new AppConfigRule()
                    {
                        Rules = new List<AppRuleTemplate>()
                        {
                            new AppRuleTemplate()
                            {
                                Action = "Hidden",
                                Name = "Rule01",
                                TargetPath = @"C:\Test\Hidden01",
                            },
                            new AppRuleTemplate()
                            {
                                Action = "Hidden",
                                Name = "Rule02",
                                TargetPath = @"C:\Test\Hidden02",
                            }
                        }
                    }
                }
            };

            return Functions.ToText(appRoot);
        }
    }
}
