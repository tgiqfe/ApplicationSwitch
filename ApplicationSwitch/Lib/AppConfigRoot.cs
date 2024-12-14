using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    internal class AppConfigRoot
    {
        [YamlMember(Alias = "App")]
        public Dictionary<string, AppConfig> Configs { get; set; }
    }
}
