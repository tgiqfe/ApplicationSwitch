using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using System.Reflection;

namespace ApplicationSwitch.Sample.SampleRule
{
    internal class Rule_File03
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
                        Name = "Rule_File03",
                        Description = "Sample Rule File",
                        Version = version,
                    },
                    Target = new AppConfigTarget()
                    {
                        EnableTargets = "PC001, PC002, PC003, PC011~100",
                        DisableTargets = "*",
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
                                TargetPath = @"%ProgramData%\Microsoft\Windows\Start Menu\Programs\Visual Studio 2022\Visual Studio Tools",
                            }
                        }
                    }
                }
            };

            return Functions.ToText(appRoot);
        }
    }
}
