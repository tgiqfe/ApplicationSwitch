using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Yml;
using ProfileList2.Lib.ScriptLanguage.Yml;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Test
{
    internal class sample_AppConfig001
    {
        public void test()
        {
            var root = new AppConfigRoot()
            {
                Configs = new Dictionary<string, AppConfig>()
                {
                    { "Adobe Acrobat Reader", new AppConfig()
                        {
                            Name = "Adobe Acrobat Reader",
                            Description = "Adobe Acrobat Reader",
                            HostNames = "PCName01",
                            Rules = new List<RuleTemplate>()
                            {
                                new RuleTemplate()
                                {
                                    Type = "FileMove",
                                    Name = "Shortcut move",
                                    Rule = "aiueo" + "\r\n" +
                                        "kakicukeko" + "\r\n" +
                                        "sasisuseso",
                                }
                            }
                        }
                    }
                }
            };

            new SerializerBuilder().
                 WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
                 WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
                 Build().
                 Serialize(Console.Out, root);
        }
    }
}
