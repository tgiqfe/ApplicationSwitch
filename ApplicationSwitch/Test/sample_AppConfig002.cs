using ApplicationSwitch.Lib.Rules;
using ApplicationSwitch.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Test
{
    internal class sample_AppConfig002
    {
        public static void Test01()
        {
            var app = new AppRoot()
            {
                Config = new AppConfig()
                {
                    Metadata = new AppConfigMetadata()
                    {
                        Name = "AppConfig02",
                        Description = "This is a sample AppConfig02",
                    },
                    Target = new AppConfigTarget()
                    {
                        EnableTargets = "aaaa",
                        DisableTargets = "bbbb",
                    },
                    Rule = new AppConfigRule()
                    {
                        Rules = new List<AppRuleTemplate>()
                        {
                            new AppRuleTemplate()
                            {
                                Action = "File",
                                Name = "Rule01",
                                Target = @"C:\Users\User\Downloads\utfunknown.zip",
                            },
                            new AppRuleTemplate()
                            {
                                Action = "File",
                                Name = "Rule02",
                                Target = @"C:\Users\User\Downloads\PSTools.zip",
                            },
                        },
                    },
                }
            };

            //app.Show();
        }
    }
}
