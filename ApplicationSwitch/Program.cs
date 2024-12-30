using ApplicationSwitch;
using ApplicationSwitch.Lib;
using System.Diagnostics;

Environment.CurrentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
var setting = DataSerializer.Load<Setting>("setting.yml");
if (!Directory.Exists(setting.LogDirectory))
{
    Directory.CreateDirectory(setting.LogDirectory);
}
Logger.LogFile =
    Path.Combine(setting.LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");

Logger.WriteLine("Start Application switch.", 0);

var path = @"..\..\..\Test\AppConfig04.yml";
var app = DataSerializer.Load<AppRoot>(path);
app.ProcessRules(setting.EvacuateDirectory);


Logger.WriteLine("End Application switch.", 0);

#if DEBUG
Console.ReadLine();
#endif