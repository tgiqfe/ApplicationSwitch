using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    /// <summary>
    /// Application configuration root.
    /// </summary>
    internal class AppRoot
    {
        [YamlMember(Alias = "App")]
        public AppConfig Config { get; set; }

        private readonly static string[] yml_extensions = new string[] { ".yml", ".yaml" };

        public static IEnumerable<AppRoot> LoadSettingFiles(string path)
        {
            if (File.Exists(path))
            {
                return new AppRoot[] { DataSerializer.Load<AppRoot>(path) };
            }
            else if (Directory.Exists(path))
            {
                return Directory.GetFiles(path).Where(x =>
                {
                    string extension = Path.GetExtension(x).ToLower();
                    return yml_extensions.Any(y => y == extension);
                }).Select(x => DataSerializer.Load<AppRoot>(x));
            }
            return Enumerable.Empty<AppRoot>();
        }

        /// <summary>
        /// Load Rule files (from ConfigDirectory parameter)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<AppRoot> LoadRuleFiles(string path)
        {
            if (File.Exists(path))
            {
                return new List<AppRoot>(new AppRoot[]
                {
                    DataSerializer.Load<AppRoot>(path)
                });
            }
            else if (Directory.Exists(path))
            {
                return Directory.GetFiles(path).Where(x =>
                {
                    string extension = Path.GetExtension(x).ToLower();
                    return yml_extensions.Any(y => y == extension);
                }).Select(x => DataSerializer.Load<AppRoot>(x)).ToList();
            }
            return new List<AppRoot>();
        }

        public void ProcessRules(string evacuateDirectory)
        {
            if (this.Config == null)
            {
                Logger.WriteLine("Config is null. (when loading config file.)");
                return;
            }
            string evacuate = Path.Combine(evacuateDirectory, this.Config.Metadata.Name);
            var endis = this.Config.Target.CheckEnDis(Environment.MachineName);
            switch (endis)
            {
                case true:
                    Logger.WriteLine($"[Enable] {this.Config.Metadata.Name}", 0);
                    foreach (var ruleTemplate in this.Config.Rule.Rules)
                    {
                        var rule = ruleTemplate.ConvertToRule(evacuate);
                        if (rule?.Enabled ?? false)
                        {
                            rule.EnableProcess();
                        }
                    }
                    break;
                case false:
                    Logger.WriteLine($"[Disable] {this.Config.Metadata.Name}", 0);
                    foreach (var ruleTemplate in this.Config.Rule.Rules)
                    {
                        var rule = ruleTemplate.ConvertToRule(evacuate);
                        if (rule?.Enabled ?? false)
                        {
                            rule.DisableProcess();
                        }
                    }
                    break;
                case null:
                    Logger.WriteLine($"[Not applicable] {this.Config.Metadata.Name}", 0);
                    break;
            }
        }
    }
}