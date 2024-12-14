

using ApplicationSwitch;
using ApplicationSwitch.Lib;
using YamlDotNet.Serialization;

var root = new AppConfigRoot()
{
    Configs = new Dictionary<string, AppConfig>()
    {
        { "Adobe Acrobat Reader", new AppConfig()
            {
                Name = "Adobe Acrobat Reader",
                Description = "Adobe Acrobat Reader",
                HostNames = "PCName01",
                Rules = new List<RuleBase>()
                {
                    new Rule_FileMove()
                    {
                        Name = "Test",
                        Type = "FileMove",
                        Index = 1,
                        TargetFilePath = @"C:\Temp\test.txt"
                    }
                }
            }
        }
    }
};



new Serializer().Serialize(Console.Out, root);


Console.ReadLine();
