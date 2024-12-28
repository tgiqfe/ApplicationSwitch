using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    internal class AppRoot
    {
        [YamlMember(Alias = "App")]
        public Dictionary<string, AppConfig> Configs { get; set; }
    }
}
