using ApplicationSwitch.Lib.Yml;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    internal class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfig Configs { get; set; }
    }
}
