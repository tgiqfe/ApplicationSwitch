using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfig Config { get; set; }

        private readonly static string[] yml_extensions = new string[] { ".yml", ".yaml" };

        public static List<AppRoot> LoadRuleFiles(string path)
        {
            if (File.Exists(path))
            {
                return new List<AppRoot>(new AppRoot[]
                {
                    Functions.Load<AppRoot>(path)
                });
            }
            else if (Directory.Exists(path))
            {
                return Directory.GetFiles(path).Where(x =>
                {
                    string extension = Path.GetExtension(x).ToLower();
                    return yml_extensions.Any(y => y == extension);
                }).Select(x => Functions.Load<AppRoot>(x)).ToList();
            }
            return new List<AppRoot>();
        }
    }
}
