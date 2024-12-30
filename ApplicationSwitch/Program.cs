using ApplicationSwitch;
using ApplicationSwitch.Lib;
using System.Diagnostics;

//  Start process
Environment.CurrentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
var setting = DataSerializer.Load<Setting>("Setting.yml");
if (!Directory.Exists(setting.LogDirectory))
{
    Directory.CreateDirectory(setting.LogDirectory);
}
Logger.LogFile =
    Path.Combine(setting.LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");
Logger.WriteLine("Start Application switch.", 0);


//  Main process
AppRoot.LoadSettingFiles(setting.ConfigDirectory).
    ToList().
    ForEach(app => app.ProcessRules(setting.EvacuateDirectory));



//var path = @"..\..\..\Test\AppConfig04.yml";
//var app = DataSerializer.Load<AppRoot>(path);
//app.ProcessRules(setting.EvacuateDirectory);

//  End process
Logger.WriteLine("End Application switch.", 0);

#if DEBUG
Console.ReadLine();
#endif